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

    public bool stopSpawning = false;
    

    UIManager _uiManager;

    private GameObject newEnemy;

    // Start is called before the first frame update
    void  Start()
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
        while (stopSpawning == false)
        {
            //ID for power ups
            //0 for triple shot
            //1 for speed
            //2 for shields
            //3 for ammo
            //4 for extra life
            //5 for Special bullet attack
            //6 for NEGATIVE Speed
            //7 for Homing Missile
            int randomSelection = Random.Range(0, 100);
            Vector3 posToSpawn = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
            if(randomSelection<=10)
                Instantiate(powerUps[0], posToSpawn, Quaternion.identity);  //about 10% chance triple shot
            if(randomSelection >10 && randomSelection <25)
                Instantiate(powerUps[1], posToSpawn, Quaternion.identity);  //about 15% chance speed
            if (randomSelection >= 25 && randomSelection < 35)
                Instantiate(powerUps[2], posToSpawn, Quaternion.identity);  //about 10% chance shields
            if (randomSelection >=35 && randomSelection < 65)
                Instantiate(powerUps[3], posToSpawn, Quaternion.identity);  //about 30% chance ammo
            if (randomSelection >=66 && randomSelection < 72)
                Instantiate(powerUps[4], posToSpawn, Quaternion.identity);  //about 6% chance extra life
            if (randomSelection >=73 && randomSelection < 81)
                Instantiate(powerUps[5], posToSpawn, Quaternion.identity);  //about 8% chance special attack
            if (randomSelection >= 81 && randomSelection < 92)
                Instantiate(powerUps[6], posToSpawn, Quaternion.identity);  //about 11% chance negative power up
            if (randomSelection >= 92 && randomSelection < 100)
                Instantiate(powerUps[7], posToSpawn, Quaternion.identity);  //about 7% chance negative power up
            float _powerupSpawn = Random.Range(4f, 10f);
            yield return new WaitForSeconds(_powerupSpawn);
        }   
    }

   public void StopSpawn()
    {
        stopSpawning = true;
    }

}
