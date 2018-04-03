/** 

* This script moves the horizontal platform between two 
* set waypoints

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovingPlatform : MonoBehaviour {

    public Transform[] Waypoints;           //set of waypoints to cycle between
    public float speed = 2;                 //speed of the platform
    private int CurrentPoint = 0;           //current point the platform is at
	

    private void FixedUpdate()
    {
        //move platform between set of waypoints
        if (transform.position.x != Waypoints[CurrentPoint].transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[CurrentPoint].transform.position, speed * Time.deltaTime);
        }

        if (transform.position.x == Waypoints[CurrentPoint].transform.position.x)
        {
            CurrentPoint += 1;
        }
        if (CurrentPoint >= Waypoints.Length)
        {
            CurrentPoint = 0;
        }
    }


}
