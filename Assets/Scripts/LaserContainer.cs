using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserContainer : MonoBehaviour
{
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       GameObject.Find("Laser(Clone)").GetComponent<Transform>().transform.parent = this.transform;

    }
}
