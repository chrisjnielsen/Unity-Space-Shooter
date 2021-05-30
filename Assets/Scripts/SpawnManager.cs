using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
   // [SerializeField]
   // private GameObject enemyPrefab;

   // [SerializeField]
   // private GameObject enemyPrefab2;
    [SerializeField]
    private GameObject[] powerUps;
    
    [SerializeField]
    private float xMax = 9f;
    [SerializeField]
    private float xMin = -9f;
    [SerializeField]
    //private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    

    UIManager _uiManager;

    private GameObject newEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<UIManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI Manager is NULL");
        }
    }

    public void StartSpawning()
    { 
        StartCoroutine(SpawnPowerRoutine());
        this.GetComponent<SpawnScriptObj>().StartEnemySpawnWaves();
        //different enemy coroutine
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnPowerRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            int randomSelection = Random.Range(0, 20);
            Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
            if(randomSelection<17)
                Instantiate(powerUps[Random.Range(0,5)], posToSpawn, Quaternion.identity);
            if(randomSelection >17)
               Instantiate(powerUps[5], posToSpawn, Quaternion.identity);
            float _powerupSpawn = Random.Range(11f, 21f);
            yield return new WaitForSeconds(_powerupSpawn);
        }   
    }

    public void PlayerWin()
    {
        _stopSpawning = true;
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
