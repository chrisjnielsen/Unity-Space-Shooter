﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public void ShieldOn()
    {
        gameObject.SetActive(true);
    }

    public void ShieldOff()
    {
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ShieldOff();
            GetComponentInParent<PolygonCollider2D>().enabled = true;
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            ShieldOff();
            GetComponentInParent<PolygonCollider2D>().enabled = true;
        }
    }
}
