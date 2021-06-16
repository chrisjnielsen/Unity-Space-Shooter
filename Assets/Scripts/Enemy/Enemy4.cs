using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : Enemy
{

    
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        CheckPlayerPosition();
    }

    void CheckPlayerPosition()
    {
        if (_player != null)
        {
            if ((int)_player.transform.position.x == (int)transform.position.x && (int)_player.transform.position.y > (int)transform.position.y)
            {
                _fireRate = Random.Range(1f, 5f);
                if (Time.time > _canFire + _fireRate)
                {
                    _enemyLaser.GetComponent<Laser>().EnemyLaserBackwards();
                    GameObject enemyLaser = Instantiate(_enemyLaser, transform.localPosition + new Vector3(0, 1.47f, 0), Quaternion.identity);
                    Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                    _canFire = Time.time;
                    for (int i = 0; i < lasers.Length; i++)
                    {
                        lasers[i].AssignEnemyLaser();
                    }
                }
                _enemyLaser.GetComponent<Laser>().TurnOffEnemyLaserBackwards();
            }
        }
    }

}
