using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport1 : MonoBehaviour
{
    public GameObject teleport2;
    public GameObject player;
    public Vector3 offset = new Vector3(2f, 0, 2f);

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.transform.position = teleport2.transform.position + offset;
        }
    }
}
