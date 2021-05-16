using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed =8f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
        

        // if object has parent, destroy parent
        if (transform.position.y > 8f)
        {
            if (this.transform.parent == true)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(gameObject);
           
        }
    }
}
