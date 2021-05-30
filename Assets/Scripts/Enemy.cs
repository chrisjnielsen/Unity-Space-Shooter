using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float xMax = 9f;
    [SerializeField]
    private float xMin = -9f;
    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    List<GameObject> waypoints;

    [SerializeField]
    private int wayPointIndex = 0;


    private UIManager _uiManager;


    private Player _player;

    private Animator _anim;
    // handle to animator
    [SerializeField]
    private AudioClip _enemyExplosion;
    private AudioSource _audioSource;
    private float _fireRate = 3f;
    private float _canFire = 3f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        if (this.CompareTag("Enemy2") == true)
        {
            //Assign enemy waypoints to instantiated prefab
            waypoints = GameManager.Instance.Waypoints;
            transform.position = waypoints[wayPointIndex].transform.position;
        }

        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }
        //handle for animator
        _anim = GetComponent<Animator>();

        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
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
        if (this.CompareTag("Enemy") == true)
        {
            CalculateMovement();
        }

        if (this.CompareTag("Enemy2") == true)
        {
            CalculateMovementNew();
        }
            
    }

    void CalculateMovement()
    {
        //for later: if enemy has been shot or damaged and animation is showing, do not teleport them to new location, let them scroll off screen and destroy
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
        }
    }

    void CalculateMovementNew()
    {
        //enemy follows set of waypoints
        transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].transform.position, _speed*2 * Time.deltaTime);
        if (transform.position == waypoints[wayPointIndex].transform.position) wayPointIndex += 1;
        if (wayPointIndex == waypoints.Count) wayPointIndex = 0;
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
        _audioSource.PlayOneShot(_enemyExplosion);
        Destroy(this.gameObject, 2f);
        yield return null;
    }
}
