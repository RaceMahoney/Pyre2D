/** 

* This script takes the images from the specifed 
* directories, using Magick.NET compares two images
* of the same index and produces a third image
* containing the highlighted differences

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
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
            if (drive == @"F:\") //need to replace with whatever the USB drive name is
            {
                destinationDrive = drive;
            }
        }
        destinationDrive += @"Comp\Control\Test#5";
    }

    private void OnApplicationQuit()
    {
        if(File.Exists(destinationDrive + @"\ValueReport.txt"))
        {
            File.Delete(destinationDrive + @"\ValueReport.txt");
        }
        Compare();
    }

    private void Compare()
    {
        var Organicpath = destinationDrive +@"\Organic";
        var Automatedpath = destinationDrive + @"\Automated";
 

        Debug.Log(Directory.GetFiles(Organicpath).Length + " in Organic. " + Directory.GetFiles(Automatedpath).Length + "in Automated");

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
                            double diff = img2.Compare(img1, ErrorMetric.StructuralDissimilarity, imgdiff);
                            imgdiff.Write(destinationDrive + @"\Differences\image_DIFF_" + i + ".png");
                            Debug.Log("Comapred " + automatedImages[i] + " with " + organicImages[i] + " to make image_Diff_" + i + ".png");
                            DisplayReport(diff, i, automatedImages[i], organicImages[i]);

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

    void DisplayReport(double diff, int i, string autoImage, string organicImage)
    {
        //change formats
        var errorPercent = diff.ToString("0.##\\%");
        var A_image = autoImage.Substring(autoImage.LastIndexOf(@"\") + 1);
        var O_image = organicImage.Substring(organicImage.LastIndexOf(@"\") + 1);


        StreamWriter writer = File.AppendText(destinationDrive + @"\ValueReport.txt");
        writer.WriteLine("image_DIFF_" + i + ".png - Error Value of: " + errorPercent);
        writer.WriteLine("Formed from " + A_image + " and " + O_image);
        writer.WriteLine();
        writer.Close();
    }
}





