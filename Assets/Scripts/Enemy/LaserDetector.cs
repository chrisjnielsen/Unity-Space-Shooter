using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") return;
    
        if (collision.tag == "Laser")
        {
            float value;

            if(this.transform.position.x < collision.transform.position.x)
            {
                value = -1.2f;
                if (Random.Range(0, 2)==0) this.GetComponentInParent<Enemy>().Dodge(value);
            }

            if(this.transform.position.x > collision.transform.position.x)
            {
                value = 1.2f;
                if(Random.Range(0,2)==0) this.GetComponentInParent<Enemy>().Dodge(value);
            }

        }
    
    }

    

}
