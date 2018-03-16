using System.Collections;
using System;
using System.IO;
using UnityEngine;

// Screen Recorder will save individual images of active scene in any resolution and of a specific image format
// including raw, jpg, png, and ppm.  Raw and PPM are the fastest image formats for saving.
//
// You can compile these images into a video using ffmpeg:
// ffmpeg -i screen_3840x2160_%d.ppm -y test.avi

public class ScreenRecorder : MonoBehaviour
{
    private Checkpoint checkpointTrigger;

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

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int counter = 0; // image #

    // commands
    private bool captureScreenshot = false;
    private bool captureVideo = false;

    public GameObject[] checkpoints;
    private bool trigger;

    private string destinationDrive;
    private string dataPath = @"";

    public Texture2D[] textures;
    private int count = 0;

    private double nextActionTime = 0.0;
    public double peroid = 3;


    void Start()
    {
        checkpointTrigger = GetComponent<Checkpoint>();

        //find the correct destination drive
        string[] drives = Directory.GetLogicalDrives();
        foreach (string drive in drives)
        {
            if (drive == @"E:\") //need to replace with whatever the USB drive name is
            {
                destinationDrive = drive;
            }
        }

        folder = destinationDrive;
    }


    // create a unique filename using a one-up variable
    private string uniqueFilename(int width, int height)
    {
        // if folder not specified by now use a good default
        if (folder == destinationDrive || folder.Length == 0)
        {
            //folder = Application.persistentDataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                var stringPath = folder + "/..";
                folder = Path.GetFullPath(stringPath);
            }
            folder += "/screenshots";

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(folder);

            // count number of files of specified format in folder
            string mask = string.Format("checkpoint_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
            counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
        }

        // use width, height, and counter for unique file name
        var filename = string.Format("{0}/checkpoint_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());

        // up counter for next call
        ++counter;

        // return unique filename
        return filename;
    }

 

    void Update()
    {

        //if x seconds have passed
        //then get the current player position and write it to a file
        if (Time.time >= nextActionTime)
        {
            nextActionTime += peroid;
            captureScreenshot = true;
        } else
        {
            captureScreenshot = false;
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
            // hide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(false);

            // create a texture to pass to encoding
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

            // put buffer into texture
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            texture.Apply();

            byte[] bytes = texture.EncodeToPNG();

            // save our test image (could also upload to WWW)
             File.WriteAllBytes(Application.dataPath + "/screenshots/image_" + count + ".png", bytes);
             count++;

            // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            DestroyObject(texture);

            // unhide optional game object if set
            if (hideGameObject != null) hideGameObject.SetActive(true);

            captureScreenshot = false;

            Debug.Log("Screenshot captured");
        }

    }


    private IEnumerator Tick()
    {
        while (true)
        {
            CaptureScreenshot();
            yield return new WaitForSeconds(10);
        }
       
    }

 
}

