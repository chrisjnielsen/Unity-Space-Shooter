using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    public float rotationSpeed = 5; //speed of turning
    public Transform ObjToFollow = null;
    public bool FollowPlayer = false;
    
    public override void Start()
    {
        base.Start();
        if (!FollowPlayer) { return; }
        if (_player != null)
        {
            ObjToFollow = _player.transform;
        }
    }
    public override void Update()
    {
        if(_player != null)
        {
            if (Vector3.Distance(_player.transform.position, transform.position) != 0)
            {
                // Get a direction vector from us to the target
                Vector3 dir = _player.transform.position - transform.position;
                // Normalize it so that it's a unit direction vector
                dir.Normalize();
                RotateTowards(_player.transform.position);
                // Move ourselves in that direction
                transform.position += dir * _speed * Time.deltaTime;
            }

        }

        


    }


    private void RotateTowards(Vector2 target)
    {
        var offset = 90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }



}



