using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private float xMin = -11.3f;
    private float xMax = 11.3f;
   

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
    private bool _isShield = false;
    [SerializeField]
    private GameObject _playerShield;

    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _fireDamage1;
    [SerializeField]
    private GameObject _fireDamage2;

    //laser audio 
    [SerializeField]
    private AudioClip _laserAudio;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _playerExplosion;


    // Start is called before the first frame update
    void Start()
    {
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

        

        if (_lives>0) CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _nextFire)
        {
            FireLaser();
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

        /*if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }

        else if (transform.position.y <= -3.7f)
        {
            transform.position = new Vector3(transform.position.x, -3.7f, 0);
        }*/
        //Math Clamp function to limit y movement
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.7f, 0), 0);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

       

        if (_isSpeedUp == true)
        {

            transform.Translate(direction * speed *2* Time.deltaTime);

        }
        else
        {   //add 75% additional speed while holding Left Shift
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(direction * speed *1.75f* Time.deltaTime);
            }
            else transform.Translate(direction * speed * Time.deltaTime);

        }

        


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
            _isShield = false;
            _playerShield.SetActive(false);
            return;

        }

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
            _spawnManager.OnPlayerDeath();
            _audioSource.PlayOneShot(_playerExplosion);
            GetComponent<BoxCollider2D>().enabled = false;
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
        _isSpeedUp = false;
    }

    public void ShieldActive()
    {
        _isShield = true;
        _playerShield.SetActive(true);


    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }


    

}
