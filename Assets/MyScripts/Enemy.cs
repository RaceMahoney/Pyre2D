using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Enemy : MonoBehaviour {

    public Transform[] Waypoints;
    public float speed = 2;
    public int CurrentPoint = 0;

    public int currentHealth = 20;
    private PlatformerCharacter2D m_Character;
    private AutoInput autoInput;
    private GameObject autoRef;


    // Use this for initialization
    void Start () {
        m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
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
        } catch (NullReferenceException e)
        {

        }
       
     
        //check current health
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //TODO death animation
        }


    }

    private void FixedUpdate()
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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_Character.Damage(1);
            //StartCoroutine(m_Character.Knockback(0.02f, 350, transform.position));
        }
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;
        //gameObject.gameObject<Animation>.Play("EnemyHurt");
    }

    
}
