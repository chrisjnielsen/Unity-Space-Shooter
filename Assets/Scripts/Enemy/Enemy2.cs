using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        waypoints = GameManager.Instance.Waypoints;
        this.transform.position = waypoints[wayPointIndex].transform.position;


    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        CalculateMovementNew();

    }


    void CalculateMovementNew()
    {
        //enemy follows set of waypoints
        transform.position = Vector3.MoveTowards(transform.position, waypoints[wayPointIndex].transform.position, _speed * 2 * Time.deltaTime);
        if (transform.position == waypoints[wayPointIndex].transform.position) wayPointIndex += 1;
        if (wayPointIndex == waypoints.Count) wayPointIndex = 0;
    }


}
