/** 

* This script controls the process of saving what is
* currently on the screen as a PNG file and save it to
* the specified location


* @author Race Mahoney
* @data 04/02/18
* @framwork .NET 3.5

*/

using System.Collections;
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;


public class ScreenRecorder : MonoBehaviour
{
    private Checkpoint checkpointTrigger;
    public GameObject player;
    public PlatformerCharacter2D character;

    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;

    private bool captureScreenshot = false;

    private string destinationDrive;
    private string dataPath;

    public Texture2D[] textures;
    private int singleDigit = 0;
    private int hundredsDigit = 0;
    private int tensDigit = 0;

    [HideInInspector]
    public List<Vector3> screenPos;
    [HideInInspector]
    public bool isReplay = false;
    private bool RecordON = false;

    private double nextActionTime = 0.0;
    public double peroid = 3;

  
    void Start()
    {
        checkpointTrigger = GetComponent<Checkpoint>();
        character = GetComponent<PlatformerCharacter2D>();
        //find the correct destination drive
        string[] drives = Directory.GetLogicalDrives();

        foreach (string drive in drives)
        {
            if (drive == @"F:\") //need to replace with whatever the USB drive name is
            {
                destinationDrive = drive;
            }
        }
        destinationDrive += @"Comp\Control\Test#5";
       
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            RecordON = false;
            Debug.Log("Recording has been turned off");

        }

        //RecordOn - only on when adding transforms to the array
        //isReplay - only on AFTER recordEvent
        //need to be looking at transforms in order during replay mode

        if (RecordON && !isReplay)
        {
            if (Time.time >= nextActionTime)
            {
                nextActionTime += peroid;
                screenPos.Add(player.transform.position);
                captureScreenshot = true;
            }
            else
            {
                captureScreenshot = false;
            }
        }

        if(!RecordON && isReplay)
        {
            try
            {
                //check the first item in the list
                float dist = Vector3.Distance(player.transform.position, screenPos[0]);
                //transform string to vector
                if (dist > 0 && dist < 0.45f)
                {
                    captureScreenshot = true;
                    //move to the next assigned vector

                    //remove the vector from the list so it cannt be used again1
                    screenPos.RemoveAt(0);

                    try
                    {
                        //turn deathScreenblocker off now that we've gotten past that dumb bug
                        character.dealthScreenBlocker = false;

                    } catch (NullReferenceException ei){
                        //ehhh
                    }
                }
                else
                    captureScreenshot = false;

            }
            catch (ArgumentOutOfRangeException e)
            {
                //Debug.Log("List is empty");
            }
        }



    }

    public void CaptureScreenshot()
    {
        captureScreenshot = true;

    }

    private void OnPostRender()
    {

        if (captureScreenshot)
        {
            Debug.Log("Screenshot Captured!");

            // hide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(false);

            // create a texture to pass to encoding
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

            // put buffer into texture
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();

            // save our test image (could also upload to WWW)
            File.WriteAllBytes(destinationDrive +  dataPath + "/image_" + hundredsDigit + tensDigit + singleDigit + ".png", bytes);
            singleDigit++;

            if(tensDigit >= 9 && singleDigit == 9)
            {
                //reset the counter
                hundredsDigit++;
                tensDigit = 0;
                singleDigit = 0;

            } else if(singleDigit >= 9)
            {
                //reset the counter to keep images in order
                tensDigit++;
                singleDigit = 0;
            }

            

            // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            DestroyObject(texture);

            // unhide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(true);

            captureScreenshot = false;

        }

    }


    public void SetOrganicPath()
    {
        dataPath = "/Organic";
        isReplay = false;
        RecordON = true;
    }

    public void SetAutomatedPath()
    {
        captureScreenshot = false; //just in case it turns to true while switching states
        dataPath = "/Automated";
        isReplay = true;
        RecordON = false;

        //remove the first item of the list on start
        //Known to cause bug
        screenPos.RemoveAt(0);
    }

 
}


