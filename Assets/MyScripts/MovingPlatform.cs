using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Transform[] Waypoints;
    public float speed = 2;
    public int CurrentPoint = 0;

    void Update()
    {
        //check if waypoints are horizontal or vertical

        //horizontal 
        if(Waypoints[0].transform.position.x == Waypoints[1].transform.position.x)
        {
            
        }

        //vertical 
        if (Waypoints[0].transform.position.y == Waypoints[1].transform.position.y)
        {
           
        }
    }

   
}
