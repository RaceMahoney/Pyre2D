using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") ;
        {
            Debug.Log("Dead");
            Application.Quit();
            //TODO death animation, SFX, and death screen
        }

    }
}
