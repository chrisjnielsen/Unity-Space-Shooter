using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointDraw : MonoBehaviour
{

    [SerializeField]
    private GameObject[] waypoints;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            for (int j = i+1; j < waypoints.Length; j++)
            {
                Gizmos.DrawSphere(waypoints[i].transform.position, 0.4f);
                Gizmos.DrawLine(waypoints[i].transform.position, waypoints[j].transform.position);
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
