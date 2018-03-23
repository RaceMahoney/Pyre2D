﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;
using System.IO;

public class CompareImages : MonoBehaviour
{

    private string oldImagePath;
    private string newImagePath;
    private int count = 0;
    private string destinationDrive;

    private List<string> organicImages = new List<string>();
    private List<string> automatedImages = new List<string>();


    // Use this for initialization
    void Start()
    {
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
        var Organicpath = Application.dataPath + "/screenshots/Organic";
        var Automatedpath = Application.dataPath + "/screenshots/Automated";

        if (Directory.GetFiles(Organicpath).Length != Directory.GetFiles(Automatedpath).Length)
        {
            //directoy sizes are not equal
            //remove from the directory that has too many files
            if(Directory.GetFiles(Automatedpath).Length > Directory.GetFiles(Organicpath).Length)
            {
                //too many files in Automated
                string[] files = Directory.GetFiles(Automatedpath);
                string firstFile = files[0];
                Debug.Log("Deleting file " + firstFile + "from Automated");
                File.Delete(firstFile);
            } else {

                // too many files in Organic 
                string[] files = Directory.GetFiles(Organicpath);
                string firstFile = files[0];
                Debug.Log("Deleting file " + firstFile + "from Organic");
                File.Delete(firstFile);
            }
        }

        //fill list with all the organic images are
        foreach (string localfile in Directory.GetFiles(Organicpath))
        {
            //make sure to get the png files and not the META files
            if (Path.GetExtension(localfile) == ".png")
                organicImages.Add(localfile);
        }

       
        //fill list with all automated images are
        foreach (string localfile in Directory.GetFiles(Automatedpath))
        {
            //make sure to get the png files and not the META files
            if (Path.GetExtension(localfile) == ".png")
                automatedImages.Add(localfile);
        }

      
        for (int i = 0; i <= organicImages.Count; i++)
        {
            //check to see if the file exits within the game and exist in the destination
            if (File.Exists(organicImages[i]) && File.Exists(automatedImages[i]))
            {
                using (var img1 = new MagickImage(automatedImages[i]))
                {
                    img1.ColorFuzz = new Percentage(65);
                    using (var img2 = new MagickImage(organicImages[i]))
                    {
                        img2.ColorFuzz = new Percentage(65);
                        using (var imgdiff = new MagickImage())
                        {
                            double diff = img2.Compare(img1, ErrorMetric.MeanSquared, imgdiff);
                            imgdiff.Write(@"D:\UnityProjects\Platformer\Pyre2D\screenshots\Differences\image_DIFF_" + i + ".png");
                            Debug.Log("image_DiFF_" + i + ".png value: " + diff);

                            if(diff < 0.05f)
                            {
                                DisplayReport(diff, i);
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Could not compare images. Whoops");
            }

           

        }
    }

    void DisplayReport(double diff, int i)
    {
        //change diff to percent
        var formatted = diff.ToString("0.##\\%");
        Debug.Log("POTENTIAL BUG LOCATED AT image_DIFF_" + i + ".png with an error value of: " + formatted);
        //Debug.Log("Consider examining playback at "); //TODO get second count from replay
    }
}





