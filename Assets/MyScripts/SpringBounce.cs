using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBounce : MonoBehaviour {

    public float thrust;
    public static GameObject PLAYER;
    public Rigidbody2D rb = PLAYER.GetComponent<Rigidbody2D>();

     //TDOD fix exceptions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            rb.velocity = transform.up * thrust;
        }

    }
}
