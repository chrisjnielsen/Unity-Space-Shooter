using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        CalculateMovement();

    }


    void CalculateMovement()
    {
        //for later: if enemy has been shot or damaged and animation is showing, do not teleport them to new location, let them scroll off screen and destroy
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -7f)
        {
            transform.position = new Vector3(Random.Range(xMin, xMax), 7.5f, 0);
        }
    }

}
