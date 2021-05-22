﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    //ID for power ups
    //0 for triple shot
    //1 for speed
    //2 for shields
    //3 for ammo
    //4 for extra life
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _powerUpSound;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                _audioSource.PlayOneShot(_powerUpSound);
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        GetComponent<CircleCollider2D>().enabled = false;
                        break;
                    case 1:
                        player.SpeedUp();
                        GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoPowerup();
                        GetComponent<BoxCollider2D>().enabled = false;
                        break;
                    case 4:
                        player.ExtraLife();
                        GetComponent<PolygonCollider2D>().enabled = false;
                        break;
                    case 5:
                        player.SpecialPower();
                        GetComponent<CircleCollider2D>().enabled = false;
                        break;
                    default:
                        break;
                }
            }
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 2f);
        }  
    }
}
