using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WayPointDrawStyle2 : MonoBehaviour
{

    [SerializeField]
    private GameObject[] waypoints = new GameObject[4];

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0, j=i+1; i < waypoints.Length-1; i++, j++)
        {   //Draw waypoints from point a to b sequentially
            Gizmos.DrawSphere(waypoints[i].transform.position, 0.4f);
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[j].transform.position);
            if (j + 1 == waypoints.Length)
            {
                Gizmos.DrawSphere(waypoints[j].transform.position, 0.4f);
                Gizmos.DrawLine(waypoints[j].transform.position, waypoints[0].transform.position);
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
