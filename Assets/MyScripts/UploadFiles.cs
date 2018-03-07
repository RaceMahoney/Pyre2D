using Firebase.Storage;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEditor;

public class UploadFiles : MonoBehaviour {

    private string destinationDrive;
    private int count = 0;
   

    private void OnApplicationQuit()
    {
        ////find the correct destination drive
        string[] drives = Directory.GetLogicalDrives();
        foreach (string drive in drives)
        {
            if (drive == @"C:\")
            {
                destinationDrive = drive;
                Debug.Log("Found " + drive);
            }
        }
        destinationDrive += @"screenshots\image_";

      

        //find the needed files in the game
        var path = Application.dataPath + "/screenshots/";
        var dirInfo = new System.IO.DirectoryInfo(path).GetFiles();

        foreach (string file in Directory.GetFiles(path))
        {
           //check to see if the file exits within the game and that it does not exist int he destination
            if (File.Exists(file) && !File.Exists(destinationDrive + count + ".png"))
            {
                File.Copy(file, destinationDrive + count + ".png");         
            }
            else
            {
                Debug.Log("Could not copy file");
            }
            count++;
           
        }

    }
   

}


