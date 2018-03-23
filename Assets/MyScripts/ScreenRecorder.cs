using System.Collections;
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

// Screen Recorder will save individual images of active scene in any resolution and of a specific image format
// including raw, jpg, png, and ppm.  Raw and PPM are the fastest image formats for saving.
//
// You can compile these images into a video using ffmpeg:
// ffmpeg -i screen_3840x2160_%d.ppm -y test.avi

public class ScreenRecorder : MonoBehaviour
{
    private Checkpoint checkpointTrigger;
    public GameObject player;

    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PPM;


    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;

    // commands
    private bool captureScreenshot = false;
    private bool captureVideo = false;

    private string destinationDrive;
    private string dataPath;

    public Texture2D[] textures;
    private int count = 0;
    private int leadingDigit = 0;

    [HideInInspector]
    public List<Vector3> screenPos;
    [HideInInspector]
    public bool isReplay = false;
    private bool RecordON = true;

    private double nextActionTime = 0.0;
    public double peroid = 3;



    void Start()
    {
        checkpointTrigger = GetComponent<Checkpoint>();
       
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            RecordON = false;
            Debug.Log("Recording has been turned off");

        }


        if (RecordON)
        {
            if (isReplay)
            {
                try
                {
                    foreach (Vector3 vect in screenPos)
                    {
                        float dist = Vector3.Distance(player.transform.position, vect);
                        //transform string to vector
                        if (dist > 0 && dist < 0.5f)
                        {
                            captureScreenshot = true;
                            //remove this vector so it is not triggered again
                            screenPos.Remove(vect);
                        }
                        else
                            captureScreenshot = false;
                    }
                }
                catch (InvalidOperationException e)
                {

                }
            }
            else
            {
                if (Time.time >= nextActionTime)
                {
                    nextActionTime += peroid;
                    screenPos.Add(player.transform.position);
                    Debug.Log(player.transform.position + " added to list");
                    captureScreenshot = true;
                }
                else
                {
                    captureScreenshot = false;
                }
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
            File.WriteAllBytes(Application.dataPath + "/screenshots" + dataPath + "/image_" + leadingDigit + count + ".png", bytes);
            count++;

            if(count >= 9)
            {
                //reset the counter to keep images in order
                leadingDigit++;
                count = 0;
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
    }

    public void SetAutomatedPath()
    {
        captureScreenshot = false; //just in case it turns to true while switching states
        dataPath = "/Automated";
        isReplay = true;
        //turn recording back on
        RecordON = true;
    }

 
}

