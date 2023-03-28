using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBasedMover : MonoBehaviour
{
    //[SerializeField] private 
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2f; //only used in non waypoint
    //[SerializeField] private bool LinearInterpolate = false;
    private int currentPoint = 0;

    private void Update()
    {
        if (Vector2.Distance(waypoints[currentPoint].transform.position, transform.position) < 0.1f)
        {
            currentPoint++;
            if(currentPoint >= waypoints.Length)
            {
                currentPoint = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, Time.deltaTime * speed);
    }
}
