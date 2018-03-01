using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Enemy : MonoBehaviour {

    public Transform[] Waypoints;
    public float speed = 2;
    public int CurrentPoint = 0;

    private int currentHealth = 20;
    private PlatformerCharacter2D m_Character;

    // Use this for initialization
    void Start () {
        m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
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

        //check current health

        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            //TODO death animation
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
