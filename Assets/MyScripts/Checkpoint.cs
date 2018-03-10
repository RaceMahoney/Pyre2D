using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Checkpoint : MonoBehaviour {

    public bool playerEnter;
    public Collider2D box;
    public float lastFrameCount;
    public float currentFrameCount;

    public void Start()
    {
        //initalize frame count
        lastFrameCount = Time.frameCount;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerEnter = true;
            Invoke("DisableBool", 0.0001f);
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

    public void DisableBool()
    {
        playerEnter = false;
    }

}
