/** 

* This script moves the vertical platform between two 
* set waypoints

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour {

    public Transform[] Waypoints;       //set of waypoints to move
    public float speed = 2;             //speed of the platform 
    public int CurrentPoint = 0;


    private void FixedUpdate()
    {
        //move between the two waypoints
        if (transform.position.y != Waypoints[CurrentPoint].transform.position.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[CurrentPoint].transform.position, speed * Time.deltaTime);
        }

        if (transform.position.y == Waypoints[CurrentPoint].transform.position.y)
        {
            CurrentPoint += 1;
        }
        if (CurrentPoint >= Waypoints.Length)
        {
            CurrentPoint = 0;
        }
    }
}
