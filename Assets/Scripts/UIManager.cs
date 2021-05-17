using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Text _restartText;

    private int ammo;
    

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
       
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }
    public void UpdateAmmo(int ammo)
    {
        
        _ammoText.text = "Ammo: " + ammo;
        


    }

    public void UpdateLives(int currentlives)
    {
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
        _gameManager.GameOver();
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

   


    

}
