using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour {

    public Transform[] Waypoints;
    public float speed = 2;
    public int CurrentPoint = 0;

    private AutoInput autoInput;
    private GameObject autoRef;




    // Use this for initialization
    void Start () {
        autoInput = GetComponent<AutoInput>();
    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            autoRef = GameObject.Find("AUTO_FireHero");
            if (autoRef.activeInHierarchy)
            {
                Time.fixedDeltaTime = autoInput.TIME;
            }
        }
        catch (NullReferenceException e)
        {

        }

    }

    private void FixedUpdate()
    {
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
