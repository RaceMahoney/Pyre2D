using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour {

    public Transform[] Waypoints;
    public float speed = 2;
    public int CurrentPoint = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
