using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    
    public float _speed = 4f;
    
    public float xMax = 9f;
    
    public float xMin = -9f;
    
    public GameObject _enemyLaser;

    
    public List<GameObject> waypoints;

    
    public int wayPointIndex = 0;


    public UIManager _uiManager;


    public Player _player;

    public Animator _anim;
    
    public AudioClip _enemyExplosion;
    public AudioSource _audioSource;
    public float _fireRate = 3f;
    public float _canFire = 3f;
    
    

    // Start is called before the first frame update
    public void Start()
    {

        
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else return;
       
        _anim = GetComponent<Animator>();

        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
    }

    public void Update()
    {
        if (Time.time> _canFire +_fireRate && _player!=null)
        {
            _fireRate = Random.Range(2f,6f);
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            _canFire = Time.time;
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }       
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                GetComponent<PolygonCollider2D>().enabled = false;
                player.Damage();   
            }
            StartCoroutine(EnemyDeath());
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            GetComponent<PolygonCollider2D>().enabled=false;
            _player.AddScore(Random.Range(5, 11));
            StartCoroutine(EnemyDeath());
        }
        
        if (other.CompareTag("Shield"))
        {
            _player.Damage();
            _player.AddScore(Random.Range(5, 11));
            StartCoroutine(EnemyDeath());
        }
        
        if (other.CompareTag("Enemy"))
        {
            return;
        }

        if (other.CompareTag("Enemy2"))
        {
            return;
        }
    }


    IEnumerator EnemyDeath()
    {   // update number of enemies in wave, and update UI
        GameManager.Instance.CurrentEnemyCount--;
        _uiManager.UpdateEnemyCount();
        _anim.SetTrigger("OnEnemyDeath");
        GetComponent<PolygonCollider2D>().enabled = false; // disable collider on death cycle so no more chance they will cause damage to Player
        _canFire =-1;
        //Animator trigger
        _audioSource.Play();
        Destroy(this.gameObject, 2f);
        yield return null;
    }
}
