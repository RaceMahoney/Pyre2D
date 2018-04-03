/** 

* This script moves and animates the enemy object

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Enemy : MonoBehaviour {

    public Transform[] Waypoints;       //waypoints for the enemy to cycle through
    public float speed = 2;             //speed of the enemy
    public int CurrentPoint = 0;

    public int currentHealth = 20;      //health of the enemy
    private PlatformerCharacter2D m_Character;
    private Vector3 startPos;           //starting position of the enemy when the game begins


    // Use this for initialization
    void Start () {
       m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        startPos = transform.position;
        
    }
	
	// Update is called once per frame
	void Update () {
       
     
        //check current health
        if(currentHealth <= 0)
        {
            //move the enemy someonewhere out of sight
            Vector3 offScreenVect = new Vector3(0, -50f, 0);
            gameObject.transform.position = offScreenVect;
            Waypoints[0].transform.position = offScreenVect;
            Waypoints[1].transform.position = offScreenVect;
            //TODO death animation
        }


    }


    private void FixedUpdate()
    {
        //move the enemy from the set of waypoints
        try
        {
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
        } catch (NullReferenceException e)
        {
            //dont worry theres nothing to see here
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_Character.Damage(1);
        }
    }

    public void Damage(int damage)
    {
        //reduce the health when damange is taken
        currentHealth -= damage;
    }

    public void TurnOn()
    {
        //turn enemy back on when entering replay mode
        if (!gameObject.activeInHierarchy)
        {
            currentHealth = 20;
            transform.position = startPos;
        } 
    }

    
}
