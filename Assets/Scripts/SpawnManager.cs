using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject enemyPrefab2;
    [SerializeField]
    private GameObject[] powerUps;
    
    [SerializeField]
    private float xMax = 9f;
    [SerializeField]
    private float xMin = -9f;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;


    private GameObject newEnemy;

    // Start is called before the first frame update
    void Start()
    {
       

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutineType1());
        StartCoroutine(SpawnEnemyRoutineType2());
        StartCoroutine(SpawnPowerRoutine());
        //different enemy coroutine
    }
    // Update is called once per frame
    void Update()
    {

       


    }

    

    IEnumerator SpawnEnemyRoutineType1()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
            newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            float _pauseSpawn = Random.Range(2f, 7f);
            yield return new WaitForSeconds(_pauseSpawn);
        }
    }


    IEnumerator SpawnEnemyRoutineType2()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            //spawn at new location off screen, to be assigned to waypoint pattern handled by GameManager
            Vector3 startpos = new Vector3(0, 15, 0);
            GameObject newEnemy = Instantiate(enemyPrefab2, startpos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            float _pauseSpawn = Random.Range(2f, 7f);
            yield return new WaitForSeconds(_pauseSpawn);
        }
    }

    IEnumerator SpawnPowerRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            int randomSelection = Random.Range(0, 20);
            
            Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
            if(randomSelection<15)
                Instantiate(powerUps[Random.Range(0,5)], posToSpawn, Quaternion.identity);
            if(randomSelection >15)
               Instantiate(powerUps[5], posToSpawn, Quaternion.identity);
            float _powerupSpawn = Random.Range(11f, 21f);
            yield return new WaitForSeconds(_powerupSpawn);
        }
           
    }

    

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
