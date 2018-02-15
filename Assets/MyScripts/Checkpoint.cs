using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Checkpoint : MonoBehaviour {

    public bool playerEnter;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerEnter = true;
        }
       
    }

    void OnTriggerExit2D(Collider2D collision)
    { 
            playerEnter = false;    
    }

    public bool getBool()
    {
        return playerEnter;
    }

}
