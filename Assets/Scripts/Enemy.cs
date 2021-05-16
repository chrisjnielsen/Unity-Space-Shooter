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
    

    private Player _player;

    private Animator _anim;
    // handle to animator
    [SerializeField]
    private AudioClip _enemyExplosion;
    private AudioSource _audioSource;

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
        //if enemy has been shot or damaged and animation is showing, do not teleport them to new location, let them scroll off screen and destroy
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4f)
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
    }

    IEnumerator EnemyDeath()
    {
        
        _anim.SetTrigger("OnEnemyDeath");
        //Animator trigger
        _audioSource.PlayOneShot(_enemyExplosion);
        Destroy(this.gameObject, 2.5f);
        yield return null;

    }

}
