using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    
    [SerializeField]
    private float _rotate;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    
 

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
       transform.Rotate(Vector3.forward * (_rotate * Time.deltaTime), Space.Self);
        

       

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            
            GameObject instance = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            GetComponent<CircleCollider2D>().enabled = false;
            //disable collider so multiple explosion sound effects aren't called
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            
            Destroy(this.gameObject, 1.1f);
            
            //Destroy(instance.gameObject, 2.3f);
        }
    }


    //check for laser collision
    //instantiate explosion at position of asteroid (us)
    //destroy explosion after 3 seconds

}
