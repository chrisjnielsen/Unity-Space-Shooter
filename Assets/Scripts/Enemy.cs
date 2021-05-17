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
    

    private Player _player;

    private Animator _anim;
    // handle to animator
    [SerializeField]
    private AudioClip _enemyExplosion;
    private AudioSource _audioSource;
    private float _fireRate = 3f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }


        //handle for animator
        _anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
       if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 8f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for(int i=0; i<lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();

            }
            
            
        }

        CalculateMovement();
    }


    void CalculateMovement()
    {

        //if enemy has been shot or damaged and animation is showing, do not teleport them to new location, let them scroll off screen and destroy
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
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

        if (other.CompareTag("Enemy"))
        {
            return;
        }
    }

    IEnumerator EnemyDeath()
    {
        
        _anim.SetTrigger("OnEnemyDeath");
        _canFire = -1f;
        //Animator trigger
        _audioSource.PlayOneShot(_enemyExplosion);
        Destroy(this.gameObject, 2.5f);
        yield return null;

    }

}
