﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("Images")]
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Image _thrustIndicator;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _missileDisplay;    
    [SerializeField]
    private Sprite[] _missileSprites;

    [Header("UI Text")]
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _cooldownText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _enemyText;
    [SerializeField]
    private Text _waveScreenText;
    [SerializeField]
    private Text _missileFireText;
    [SerializeField]
    private Text _winText;


    [Header("Variables")]
    [SerializeField]
    private float _thrustTimer = 1f;
    [SerializeField]
    private float _maxThrusterTimer = 1f;
    [SerializeField]
    private KeyCode _selectKey = KeyCode.LeftShift;
    
    private int wavecurrent;
    private int wavetotal;


    private bool _shouldUpdateThruster = false;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("player is null");
        }
        _cooldownText.enabled = false;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _winText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateWaveScreenText()
    {
        if (wavecurrent < wavetotal) StartCoroutine(ShowWaveText(wavecurrent));
        if (wavecurrent == wavetotal) StartCoroutine(ShowWaveCompleteText());
    }

    IEnumerator ShowWaveText(int currentwave)
    {
        _waveScreenText.gameObject.SetActive(true);
        _waveScreenText.text = "Wave " + currentwave + " Complete! Next Wave...";
        yield return new WaitForSeconds(2f);
        _waveScreenText.gameObject.SetActive(false);

    }

    IEnumerator ShowWaveCompleteText()
    {
        _waveScreenText.gameObject.SetActive(true);
        _waveScreenText.text = "All Waves Completed! Good Job!";
        yield return null;
    }

    

    public void UpdateEnemyCount()
    {
        
        _enemyText.text = "Enemies: " + (GameManager.Instance.CurrentEnemyCount);
        if (GameManager.Instance.CurrentEnemyCount == 0)
        {
            UpdateWaveScreenText();
            
        }
    }

    public void UpdateWaves(int waveCurrent, int waveTotal)
    {
        wavecurrent = waveCurrent;
        wavetotal = waveTotal;
        _waveText.text = "Wave:  " + waveCurrent + "  /  " + waveTotal;
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }
    public void UpdateAmmo(int ammoCurrent, int ammoTotal)
    {
         _ammoText.text = "Ammo:  " + ammoCurrent +"  /  " + ammoTotal;
    }

    public void UpdateThruster()
    {
        
        //Show radial dial for thruster in use, hide when not in use and also hide when use is exceeded and need to cool down
        //Show cooldown text when thruster timer used up, or just holding in thruster key, reset when thruster key lifted
        if (Input.GetKey(_selectKey))
        {
            player.ThrusterOn();
            _cooldownText.enabled = false;
            _shouldUpdateThruster = false;
            _thrustTimer -= Time.deltaTime;
            _thrustIndicator.enabled = true;
            _thrustIndicator.fillAmount = _thrustTimer;

            if (_thrustTimer <= 0)
            {
                player.ThrusterOff();
                _thrustTimer = 0;
                _thrustIndicator.fillAmount = 0;
                _thrustIndicator.enabled = false;
                _cooldownText.enabled = true;
            }
        }
        else
        {
            if (_shouldUpdateThruster)
            {
                _cooldownText.enabled = false;
                player.ThrusterOn(); 
                _thrustTimer += Time.deltaTime;
                _thrustIndicator.fillAmount = _thrustTimer;
                if(_thrustTimer >= _maxThrusterTimer)
                {
                    player.ThrusterOff();
                    _cooldownText.enabled = false;
                    _thrustIndicator.enabled = false;
                    _shouldUpdateThruster = false;
                }
            }
        }

        if(Input.GetKeyUp(_selectKey))
        {
            _shouldUpdateThruster = true;
        }
    }

    
    public void UpdateMissiles(int currentMissiles)
    {
        if (currentMissiles > 6) currentMissiles = 6;
        _missileDisplay.sprite = _missileSprites[currentMissiles];
        if (currentMissiles > 0) _missileFireText.gameObject.SetActive(true);
        if (currentMissiles == 0) _missileFireText.gameObject.SetActive(false);
    }


    public void UpdateLives(int currentlives)
    {
        //make sure max lives cannot go above 3
        if (currentlives > 3) currentlives = 3;
        _livesImage.sprite = _livesSprites[currentlives];
        if (currentlives == 0)
        {
            GameOverSequence();
        }
    }
    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
        GameManager.Instance.GameOver();
    }


    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator GameWinFlicker()
    {
        while (true)
        {
            _winText.text = "WINNER! ALL STAGES CLEARED \n THANK YOU FOR PLAYING!";
            yield return new WaitForSeconds(0.5f);
            _winText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }


    public void GameWin()
    {
        _winText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameWinFlicker());
        GameManager.Instance.GameOver();
    }



}
