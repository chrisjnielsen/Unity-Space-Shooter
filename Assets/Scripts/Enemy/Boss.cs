using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : Enemy
{
    [SerializeField]
    private ParticleSystem _damage1PS;
    [SerializeField]
    private ParticleSystem _damage2PS;
    [SerializeField]
    private ParticleSystem _damage3PS;
    [SerializeField]
    private ParticleSystem _damage4PS;

    [SerializeField]
    private BoxCollider2D _bossHit1Collider;
    [SerializeField]
    private BoxCollider2D _bossHit2Collider;
    [SerializeField]
    private BoxCollider2D _bossHit3Collider;
    [SerializeField]
    private BoxCollider2D _bossHit4Collider;

    private new Animator _anim;

    
    
    public bool _isBossStage1Active = false;
    public bool _isBossStage2Active = false;
    public bool _isBossStage3Active = false;
    public bool _isBossStage4Active = false;

    public int _hitCount;

    private SpawnManager _spawnManager;
    

    [SerializeField]
    private float _bossSpeed = 2f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _bossHit1Collider = gameObject.transform.Find("BossHit1").GetComponent<BoxCollider2D>();
        _bossHit2Collider = gameObject.transform.Find("BossHit2").GetComponent<BoxCollider2D>();
        _bossHit3Collider = gameObject.transform.Find("BossHit3").GetComponent<BoxCollider2D>();
        _bossHit4Collider = gameObject.transform.Find("BossHit4").GetComponent<BoxCollider2D>();
        _damage1PS = gameObject.transform.Find("Damage1").GetComponent<ParticleSystem>();
        _damage2PS = gameObject.transform.Find("Damage2").GetComponent<ParticleSystem>();
        _damage3PS = gameObject.transform.Find("Damage3").GetComponent<ParticleSystem>();
        _damage4PS = gameObject.transform.Find("Damage4").GetComponent<ParticleSystem>();
        _damage1PS.Stop();
        _damage2PS.Stop();
        _damage3PS.Stop();
        _damage4PS.Stop();
        _bossHit1Collider.enabled = true;
        
        _bossHit2Collider.enabled = false;
        _bossHit3Collider.enabled = false;
        _bossHit4Collider.enabled = false;

        _anim = GetComponent<Animator>();
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnManager>();
        BossStage1();
        waypoints = GameManager.Instance.BossWaypoints;
        this.transform.position = waypoints[wayPointIndex].transform.position;

        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
        _anim.Play("BossDamage0_anim");
    }


    public override void Update()
    {

        if (Time.time > _canFire + _fireRate && _player != null && _spawnManager.stopSpawning==false)
        {
            _fireRate = Random.Range(1f, 5f);
            GameObject enemyLaser = Instantiate(_enemyLaser, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            _canFire = Time.time;
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

        if(_spawnManager.stopSpawning == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].transform.position, _bossSpeed * Time.deltaTime);
            if (transform.position == waypoints[wayPointIndex].transform.position) wayPointIndex += 1;
            if (wayPointIndex == waypoints.Count) wayPointIndex = 0;
        }
    }

    public void BossStage1()
    {
        _isBossStage1Active = true;
        _hitCount = 0;
        
    }

    public void BossStage1Over()
    { 
        _anim.Play("BossDamage1_anim");
        _damage1PS.Play();
        _bossHit1Collider.gameObject.SetActive(false);
        BossStage2();
    }

    public void BossStage2()
    {
        _isBossStage2Active = true;
        _hitCount = 0;
        _bossHit2Collider.enabled=true;
    }

    public void BossStage2Over()
    {
        _anim.Play("BossDamage2_anim");
        _damage2PS.Play();
        _bossHit2Collider.gameObject.SetActive(false);
        BossStage3();
    }

    public void BossStage3()
    {
        _isBossStage3Active = true;
        _hitCount = 0;
        _bossHit3Collider.enabled = true;
    }

    public void BossStage3Over()
    {
        _anim.Play("BossDamage3_anim");
        _damage3PS.Play();
        _bossHit3Collider.gameObject.SetActive(false);
        BossStage4();
    }

    public void BossStage4()
    {
        _isBossStage4Active = true;
        _hitCount = 0;
        _bossHit4Collider.enabled = true;
    }

    public void BossStage4Over()
    {
        _anim.Play("BossDamage4_anim");
        _damage4PS.Play();
        _bossHit4Collider.gameObject.SetActive(false);
        _bossSpeed = 0;
        _spawnManager.StopSpawn();
        _uiManager.GameWin();
    }


}
