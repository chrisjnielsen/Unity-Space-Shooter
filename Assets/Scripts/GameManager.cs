using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver;

    List<GameObject> _wayPoints = new List<GameObject>();
    public List<GameObject> Waypoints { get { return _wayPoints; } set { _wayPoints = value; } }


    //Game Manager can be accessed globally, holds references to waypoints, to then assign to Enemy

    static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            return _instance;
        }
        set { _instance = value; }
    }

    int currentEnemyCount;
    // Getter Setter for tracking enemy count and advancing the spawn waves
    public int CurrentEnemyCount
    {
        get { return currentEnemyCount; }
        set
        {
            currentEnemyCount = value;
            if (currentEnemyCount <= 0)
            {
                SpawnScriptObj.Instance._currentWave++;
                SpawnScriptObj.Instance.StartEnemySpawnWaves();
                
            }
        }
    }

    void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {//find Waypoints to assign to Enemy that are instantiated prefabs
        GameObject gameObjects = GameObject.Find("WaypointContainer1");
       
        foreach(Transform child in gameObjects.transform)
        {
            _wayPoints.Add(child.gameObject);   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver==true)
        {
            SceneManager.LoadScene(1);
        }

        //if escape key quit application

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void GameOver()
    {
        _isGameOver = true;
    }

}
