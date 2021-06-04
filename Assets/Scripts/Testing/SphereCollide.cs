using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCollide : MonoBehaviour
{
    public float magnitude = 3f;
    public float initialCollision;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.back * initialCollision);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        float rayDrawDistance = 5f;
        Debug.DrawRay(
            start: collision.contacts[0].point,
            dir: collision.contacts[0].normal * rayDrawDistance,
            Color.red,
            duration: 0.5f);

        rb.AddForce(collision.contacts[0].normal * magnitude);
       
    }

}
