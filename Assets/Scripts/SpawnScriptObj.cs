using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScriptObj : MonoBehaviour
{
        //SpawnScriptObj may be accessed globally to send out the spawn waves
    static SpawnScriptObj _instance;
    public static SpawnScriptObj Instance
     {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(SpawnScriptObj)) as SpawnScriptObj;
            }
        return _instance;
        }
        set { _instance = value; }
    }

    [SerializeField]
    private List<Wave> _waves = new List<Wave>();

    UIManager _uiManager;
    SpawnManager _spawnManager;

    public int _currentWave;

    [SerializeField]
    private float xMax = 9f;
    [SerializeField]
    private float xMin = -9f;
    [SerializeField]
    private GameObject _enemyContainer;
    

    public static int enemyCount;


    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("Spawn").GetComponent<SpawnManager>();

        _currentWave = 0;
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartEnemySpawnWaves()
    {
        StartCoroutine(StartWaveRoutine());
    }

    IEnumerator StartWaveRoutine()
    {
        yield return new WaitForSeconds(2f);

        if (_currentWave == _waves.Count)
        {
            _spawnManager.PlayerWin();
        }
        else
        {
            var currentWave = _waves[_currentWave].sequence;
            enemyCount = currentWave.Count;
            GameManager.Instance.CurrentEnemyCount = enemyCount;    //assign total enemy count from spawn wave, then subtract as each enemy destroyed
            _uiManager.UpdateWaves(_currentWave + 1, _waves.Count);
            _uiManager.UpdateEnemyCount();

            foreach (var obj in currentWave)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
                Instantiate(obj, posToSpawn, Quaternion.identity);
                float _pauseSpawn = Random.Range(2f, 7f);
                yield return new WaitForSeconds(_pauseSpawn);
            }
        }
    }
}
