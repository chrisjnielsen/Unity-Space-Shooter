using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    Player _player;
    [SerializeField]
    private float _speed = 3f;
    //ID for power ups
    //0 for triple shot
    //1 for speed
    //2 for shields
    //3 for ammo
    //4 for extra life
    //5 for Special bullet attack
    //6 for NEGATIVE Speed
    [SerializeField]
    private int powerupID;
    private AudioSource _audioSource;
    private bool _canCollectPickup;
    private float _pickupMoveSpeed = 4f;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _canCollectPickup = false;

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        else return;
    }

    // Update is called once per frame
    void Update()
    { 
        if (_canCollectPickup)
        {
            if (_player != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _pickupMoveSpeed * Time.deltaTime);
                Vector2 dist = _player.transform.position - transform.position;
                if (dist.magnitude <= 0.5f) StartCoroutine(CoolDown());
            }
            
        }
        else transform.Translate(Vector2.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    public void CanCollectPickup()
    {
        _canCollectPickup = true;  
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(5f);
        _canCollectPickup = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                _audioSource.Play();
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        GetComponent<CircleCollider2D>().enabled = false;
                        break;
                    case 1:
                        player.SpeedUp();
                        GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoPowerup();
                        GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    case 4:
                        player.ExtraLife();
                        GetComponent<PolygonCollider2D>().enabled = false;
                        break;
                    case 5:
                        player.SpecialPower();
                        GetComponent<CircleCollider2D>().enabled = false;
                        break;
                    case 6:
                        player.NegativeSpeed();
                        GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    default:
                        break;
                }
            }
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 2f);
        }
        else if (other.CompareTag("EnemyLaser"))
        {
            Destroy(gameObject);
        }
    }
}
