/** 

* This script controls ladder movements when player is colliding with the ladder

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour {

    public float speed = 10f;
    public float stayForce = 1.5f;


    private void OnTriggerStay2D(Collider2D collision)
    {
        try
        {
            if (collision.tag == "Player" && Input.GetKey(KeyCode.UpArrow))
            {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
            }

            else if (collision.tag == "Player" && Input.GetKey(KeyCode.DownArrow))
            {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
            }

            else
            {
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, stayForce);
            }
        }catch (MissingComponentException e)
        {
            Debug.Log("Missing Component?");
        }
    }
}
