using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour {

    public float speed = 10f;
    public float stayForce = 1.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
