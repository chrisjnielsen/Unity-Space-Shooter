using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }
    private void FixedUpdate()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(direction * speed * Time.deltaTime);

    }

}
