using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;
using System.IO;

public class CompareImages : MonoBehaviour {

    private string oldImagePath;
    private string newImagePath;
    private int count = 0;
    private string destinationDrive;
   
    private List<string> localImages = new List<string>();
    private List<string> destImages = new List<string>();


    // Use this for initialization
    void Start () {
        //find the correct destination drive
        string[] drives = Directory.GetLogicalDrives();

        foreach (string drive in drives)
        {
            if (drive == @"E:\") //need to replace with whatever the USB drive name is
            {
                destinationDrive = drive;
            }
        }
        destinationDrive += @"screenshots";   
    }

    private void OnApplicationQuit()
    {
        Compare();
    }

    private void Compare()
    {
        var localpath = Application.dataPath + "/screenshots/";
        var local_dirInfo = new System.IO.DirectoryInfo(localpath).GetFiles();

        //fill list with all the local images are
        foreach (string localfile in Directory.GetFiles(localpath))
        {
            localImages.Add(localfile);
        }

        //fill list with all local images are
        foreach (string destfile in Directory.GetFiles(destinationDrive))
        {
            //destImages.Add(destfile);
            Debug.Log(destfile);
        }

        //for (int i = 0; i <= localImages.Count; i++)
        //{
        //    //check to see if the file exits within the game and that it does not exist int the destination
        //    if (File.Exists(localImages[i]) && !File.Exists(destImages[i]))
        //    {
        //        using (var img1 = new MagickImage(destImages[i]))
        //        {
        //            using (var img2 = new MagickImage(localImages[i+1]))
        //            {
        //                using (var imgdiff = new MagickImage())
        //                {
        //                    double diff = img1.Compare(img2, new ErrorMetric(), imgdiff);
        //                    imgdiff.Write(@"D:\UnityProjects\Platformer\Pyre2D\screenshots\Differences\image_DIFF_" + i + ".png");
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("Could not compare images. Whoops");
        //    }
            

        
    }
}





