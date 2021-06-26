using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float speed = 5f;
    private float xMin = -11.3f;
    private float xMax = 11.3f;

    private int _shieldStrength = 3;
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private Vector3 offset = new Vector3(0, 0.8f, 0);
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _nextFire = 0f;
    private bool _isTripleShot = false;

    public int _lives = 3;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private bool _isSpeedUp = false;
    private bool _isNegativeSpeed = false;
    private bool _isShield = false;
    [SerializeField]
    private GameObject _playerShield;


    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _fireDamage1;
    [SerializeField]
    private GameObject _fireDamage2;

    private int _currentLaserCount;
    private int _totalLaserCount;
    private bool _ammoEmpty;
    [SerializeField]
    private GameObject[] laserBurst = new GameObject[100];
    [SerializeField]
    private GameObject _thrusters;

    //laser audio 
    [SerializeField]
    private AudioClip _laserAudio;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _playerExplosion;

    private float holdTime;
    private bool _canUseThrusters;

    [SerializeField]
    private GameObject _missilePrefab;
    [SerializeField]
    private bool _missileActive;
    private int _missileCount;
    private int _missileMax = 6;
    [SerializeField]
    private float _missileFireRate = 0.8f;


    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
    }


    // Start is called before the first frame update
    void Start()
    {

        _missileCount = 0;
        _canUseThrusters = true;
        _thrusters.GetComponent<SpriteRenderer>().enabled = false;
        _ammoEmpty = false;
        _currentLaserCount = 60;
        _totalLaserCount = 60;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL");
        }
        else _audioSource.clip = _laserAudio;
        _lives = 3;
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
        _uiManager.UpdateLives(_lives);
        _uiManager.UpdateAmmo(_currentLaserCount, _totalLaserCount);
        _uiManager.UpdateMissiles(_missileCount);

        transform.position = new Vector3(0, -3.7f, 0);
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_lives > 0) CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire && _ammoEmpty == false)
        {
            FireLaser();
            _currentLaserCount--;
            if (_currentLaserCount < 1)
            {
                _currentLaserCount = 0;
                _ammoEmpty = true;
            }
            _uiManager.UpdateAmmo(_currentLaserCount, _totalLaserCount);
        }
        else if ((Input.GetKey(KeyCode.V) && Time.time > _nextFire && _missileActive == true && FindClosestEnemy() != null))
        {
            _nextFire = Time.time + _missileFireRate;   
           
            if (_missileCount == 0)
            {
                _missileActive = false;
            }
            else if (_missileCount > 0)
            {
                Instantiate(_missilePrefab, transform.position, Quaternion.identity);
                _missileCount--;
                _uiManager.UpdateMissiles(_missileCount);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            var _pickup = GameObject.FindGameObjectWithTag("Powerup").GetComponent<Powerup>();
            _pickup.CanCollectPickup();
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (transform.position.x > xMax)
        {
            transform.position = new Vector3(xMin, transform.position.y, 0);
        }

        else if (transform.position.x < xMin)
        {
            transform.position = new Vector3(xMax, transform.position.y, 0);
        }

        //Math Clamp function to limit y movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.7f, 4f), 0);
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

       


        if (_isSpeedUp == true)
        { 
            transform.Translate(direction * speed * 2 * Time.deltaTime);
        }
        else if (_isNegativeSpeed == true)
        {
            transform.Translate(direction * speed * 0.1f * Time.deltaTime);
        }
        else
        {   //add 75% additional speed while holding Left Shift
            //and check for if there is a cooldown on use of thrusters
            if (Input.GetKey(KeyCode.LeftShift) && _canUseThrusters == true)
            {
                _thrusters.GetComponent<SpriteRenderer>().enabled = true;
                transform.Translate(direction * speed * 1.75f * Time.deltaTime);
                _uiManager.UpdateThruster();
            }
            else
            {
                _thrusters.GetComponent<SpriteRenderer>().enabled = false;
                _uiManager.UpdateThruster();
                transform.Translate(direction * speed * Time.deltaTime);
            }
        }
    }

    public void MissilePower()
    {
        _missileCount += 3;
        _missileActive = true;
        if (_missileCount > _missileMax) _missileCount = _missileMax;
        _uiManager.UpdateMissiles(_missileCount);
    }


    GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in enemies)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance)
    
        {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }






    public void ThrusterOn()
    {
        _canUseThrusters = true;
    }

    public void ThrusterOff()
    {
        _canUseThrusters = false;
    }


    void FireLaser()
    {
        if(_isTripleShot == true && _lives >0)
        {
            Instantiate(_tripleShotPrefab, transform.position + offset, Quaternion.identity);
            _audioSource.Play();
        }
        else if (_lives >0)
        {
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            _audioSource.Play();
        }
        _nextFire = Time.time+ _fireRate;


        //figure out how to store laser prefabs in empty gameobject container, similar to enemy prefabs
        //laser prefabs need to retain their own movement, and not move with parent (Player)
    }

    public void Damage()
    {
        if (_isShield == true)
        {       
            _shieldStrength--;
            
            if (_shieldStrength == 2) _playerShield.GetComponent<SpriteRenderer>().color = new Color(.66f, .66f, .66f);
            if (_shieldStrength == 1) _playerShield.GetComponent<SpriteRenderer>().color = new Color(.33f, .33f, .33f);
            if (_shieldStrength == 0)
            {
                _isShield = false;
                _playerShield.SetActive(false);
                GetComponent<PolygonCollider2D>().enabled = true;
            }
            return;
        }
        // camera shake effect called
        mainCamera.gameObject.SendMessage("TriggerShake");
        _lives -= 1;

        if (_lives == 2)
        {
            _fireDamage1.SetActive(true);
        }

        if (_lives == 1)
        {
            _fireDamage2.SetActive(true);
        }

        if (_lives < 0) _lives = 0;
        _uiManager.UpdateLives(_lives);
        
        if (_lives < 1)
        {
            SpawnScriptObj.Instance.OnPlayerDeath();
            _audioSource.PlayOneShot(_playerExplosion);
            GetComponent<PolygonCollider2D>().enabled = false;
            Destroy(this.gameObject,2f);
        }
    }

   
    public void TripleShotActive()
    {
        _isTripleShot = true;
        StartCoroutine(PowerDown());
    }

    IEnumerator PowerDown()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShot = false;
    }

    public void SpeedUp()
    {
        _isSpeedUp = true;
        StartCoroutine(SpeedDown());
    }

    IEnumerator SpeedDown()
    {
        yield return new WaitForSeconds(5f);
        _canUseThrusters = true;
        _isNegativeSpeed = false;
        _isSpeedUp = false;
    }

    public void NegativeSpeed()
    {
        _isNegativeSpeed = true;
        _canUseThrusters = false;
        StartCoroutine(SpeedDown());
    }

    public void ShieldActive()
    {
        _isShield = true;
        _playerShield.SetActive(true);
        
        _shieldStrength = 3;
        _playerShield.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }
    public void AmmoPowerup()
    {
        _currentLaserCount += 15;
        _ammoEmpty = false;
        if (_currentLaserCount >= _totalLaserCount) _currentLaserCount = _totalLaserCount;
        _uiManager.UpdateAmmo(_currentLaserCount, _totalLaserCount);
    }

    public void ExtraLife()
    {
        _lives = _lives + 1;
        //turn back off damage visuals if life restored
        if (_lives == 3) _fireDamage1.SetActive(false);
        else if (_lives == 2) _fireDamage2.SetActive(false);
        _uiManager.UpdateLives(_lives);
    }

    public void SpecialPower()
    {
        StartCoroutine(LaserShot());   
    }

    IEnumerator LaserShot()
    {
        for (int i = 0; i < 50; i++)
        {
            laserBurst[i] = Instantiate(_laserPrefab as GameObject, transform.position, Quaternion.Euler(0, 0, 30f * i));
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

   

    

}
