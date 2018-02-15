using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovingPlatform : MonoBehaviour {

    public Transform[] Waypoints;
    public float speed = 2;
    private int CurrentPoint = 0;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
