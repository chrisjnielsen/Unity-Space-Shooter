using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    public override void Start()
    {
        base.Start();
        waypoints = GameManager.Instance.Waypoints;
        this.transform.position = waypoints[wayPointIndex].transform.position;
    }
    public override  void Update()
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
