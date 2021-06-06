using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 4f;
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
    
    // Start is called before the first frame update
    public virtual void Start()
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
        
        if (Random.Range(0, 50) > 40)
        {
            GameObject enemyShield = Instantiate(_shieldPrefab, transform.position, Quaternion.identity);
            enemyShield.transform.parent = this.transform;
            enemyShield.GetComponent<Shield>().ShieldOn();
            this.GetComponent<PolygonCollider2D>().enabled = false;
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
    }
   
    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (other.gameObject.tag)
        {
            case "Player":
                if(_hasShield == true)
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
                if(_hasShield == true)
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
                if(_hasShield == true)
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
                
            default:
                break;
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
