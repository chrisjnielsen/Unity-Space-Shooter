using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private int _bossShieldHit = 0;

     void Start()
    {
        if (transform.parent.CompareTag("Boss"))
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }    
    }

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
        }

        if (other.CompareTag("Laser") && transform.parent.CompareTag("Boss"))
        {
            _bossShieldHit++;
            Destroy(other.gameObject);
            if (_bossShieldHit == 5)
            {
                ShieldOff();
            }
        }

        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            ShieldOff();
        }
        if (other.CompareTag("HomingMissile"))
        {
            ShieldOff();
        }

        

    }
}
