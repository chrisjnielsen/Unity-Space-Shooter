using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
   

    [SerializeField]
    protected float _speed = 2.5f;
    [SerializeField]
    protected float xMax = 9f;
    [SerializeField]
    protected float xMin = -9f;
    [SerializeField]
    protected GameObject _enemyLaser;
    [SerializeField]
    protected GameObject _shieldPrefab;
    [SerializeField]
    protected List<GameObject> waypoints;
    [SerializeField]
    protected int wayPointIndex = 0;
    protected UIManager _uiManager;
    protected Player _player;
    protected Animator _anim;
    protected AudioClip _enemyExplosion;
    protected AudioSource _audioSource;
    [SerializeField]
    protected float _fireRate = 3f;
    [SerializeField]
    protected float _canFire = 3f;
    [SerializeField]
    protected bool _hasShield = false;
    [SerializeField]
    protected bool _followPlayer = false;
    [SerializeField]
    protected Vector3 _startPosition;
    [SerializeField]
    protected int _distToPowerup = 10;
    [SerializeField]
    protected Quaternion _startRotation;
    protected Quaternion _originalRotationValue;
    protected Rigidbody2D rb;
    protected float _rotateSpeed = 1f;

    
    // Start is called before the first frame update
    public virtual void Start()
    {
        _originalRotationValue = transform.rotation;
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

        if (Random.Range(0, 50) > 40)
        {
            GameObject enemyShield = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
            enemyShield.transform.parent = this.transform;
            enemyShield.GetComponent<Shield>().ShieldOn();
            _hasShield = true;
        }
    }

    public virtual void Update()
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

        if(_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) > 3f) _followPlayer = false;
            if (Vector3.Distance(_player.transform.position, transform.position) <= 3f) _followPlayer = true;
            CalculateMovement();
        }
        CheckHitPowerUp();
    }

    public virtual void CalculateMovement()
    {
        switch (_followPlayer)
        {
            case true:
                // Get a direction vector from us to the target
                Vector3 dir = _player.transform.position - transform.position;
                // Normalize it so that it's a unit direction vector
                dir.Normalize();
                RotateTowards(_player.transform.position);
                // Move ourselves in that direction
                transform.position += dir * _speed * Time.deltaTime;
                break;
            case false:  
                //restore default rotation and movement
                transform.rotation = Quaternion.Slerp(transform.rotation, _originalRotationValue, Time.deltaTime * _speed);
                transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);
                if (transform.position.y < -7f)
                {
                    transform.position = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
                }
                break;
        }
    }

    public virtual void Dodge(float value)
    {
        transform.position = new Vector2(transform.position.x + value, transform.position.y);
    }

    public virtual void RotateTowards(Vector2 target)
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       switch (other.gameObject.tag)
            {
                case "Player":
                    if (_hasShield == true)
                    {
                        _player.Damage();
                        _hasShield = false;
                    }
                    else if (_hasShield == false)
                    {
                        GetComponent<PolygonCollider2D>().enabled = false;
                        _player.Damage();
                        StartCoroutine(EnemyDeath());
                    }
                    break;
                case "Laser":
                    if (_hasShield == true)
                    {
                        Destroy(other.gameObject);
                        _hasShield = false;
                    }
                    else if (_hasShield == false)
                    {
                        Destroy(other.gameObject);
                        GetComponent<PolygonCollider2D>().enabled = false;
                        _player.AddScore(Random.Range(5, 11));
                        StartCoroutine(EnemyDeath());
                    }
                    break;
                case "Shield":
                    if (_hasShield == true)
                    {
                        _hasShield = false;
                        return;
                    }
                    else if (_hasShield == false)
                    {
                        _player.Damage();
                        _player.AddScore(Random.Range(5, 11));
                        StartCoroutine(EnemyDeath());
                    }
                    break;
                case "Enemy":
                    return;

                case "Enemy2":
                    return;

                case "HomingMissile":
                {
                    if(_hasShield == true)
                    {
                        Destroy(other.gameObject);
                        _hasShield = false;
                    }
                    else if (_hasShield == false)
                    {
                        Destroy(other.gameObject);
                        _player.AddScore(Random.Range(10, 20));
                        StartCoroutine(EnemyDeath());
                    }
                    break;
                }

                default:
                    break;   
       } 
    }

    void CheckHitPowerUp()
    {        
        RaycastHit2D hit;
        Debug.DrawRay(transform.position, Vector3.down * _distToPowerup);
        hit = Physics2D.Raycast(transform.position, Vector2.down,_distToPowerup);

        if (hit.collider == null)
        {
            return;
        }
        else if (hit.collider.tag == "Powerup")
        {
            if (Time.time > _canFire + _fireRate)
            {
                _fireRate = Random.Range(0f, 1f);
                GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                _canFire = Time.time;
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        } 
    }

    IEnumerator EnemyDeath()
    {   // update number of enemies in wave, and update UI
        GameManager.Instance.CurrentEnemyCount--;
        _uiManager.UpdateEnemyCount();
        transform.Translate(Vector3.zero);
        //Animator trigger
        _anim.SetTrigger("OnEnemyDeath");
        GetComponent<PolygonCollider2D>().enabled = false; // disable collider on death cycle so no more chance they will cause damage to Player
        _canFire =-1;
        _audioSource.Play();
        Destroy(this.gameObject, 1.6f);
        yield return null;
    }
}
