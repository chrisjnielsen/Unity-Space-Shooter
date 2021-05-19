using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject[] powerUps;
    
    [SerializeField]
    private float xMax = 9f;
    [SerializeField]
    private float xMin = -9f;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    


    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerRoutine());

    }
    // Update is called once per frame
    void Update()
    {

       


    }

    // spawn game objects every 5 seconds.
    // create coroutine of IEnumber -- yield events
    // while loop

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            float _pauseSpawn = Random.Range(2f, 5f);
            yield return new WaitForSeconds(_pauseSpawn);
        }
    }


    IEnumerator SpawnPowerRoutine()
    {
        yield return new WaitForSeconds(2f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
            GameObject newPowerup = Instantiate(powerUps[Random.Range(0,5)], posToSpawn, Quaternion.identity);
            float _powerupSpawn = Random.Range(11f, 21f);
            yield return new WaitForSeconds(_powerupSpawn);
        }
           
    }

    

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
