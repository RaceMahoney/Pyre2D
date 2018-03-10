using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImageMagick;
using System.IO;

public class CompareImages : MonoBehaviour {

    private string oldImagePath;
    private string newImagePath;
    private int count = 0; 


	// Use this for initialization
	void Start () {
        Compare();
	}
	
	void Compare()
    {

        var path = Application.dataPath + "/screenshots/";
        var dirInfo = new System.IO.DirectoryInfo(path).GetFiles();

        foreach (string file in Directory.GetFiles(path))
        {
            //check to see if the file exits within the game and that it does not exist int he destination
            if (File.Exists(file) && !File.Exists(path + "image_" + count + ".png"))
            {
                using (var img1 = new MagickImage(path + "image_" + count + ".png"))
                {
                    using (var img2 = new MagickImage(path + "image_" + count + ".png"))
                    {
                        using (var imgdiff = new MagickImage())
                        {
                            double diff = img1.Compare(img2, new ErrorMetric(), imgdiff);
                            imgdiff.Write(@"D:\UnityProjects\Platformer\Pyre2D\screenshots\Differences\image_DIFF_" + count + ".png");
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Could not copy file");
            }
            count += 2;
        }




        
    }
}
