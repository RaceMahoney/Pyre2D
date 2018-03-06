using Firebase.Storage;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEditor;

public class UploadFiles : MonoBehaviour {

    private string destinationDrive;
    private string dataPath = @"";
    private string inputSeq = "inputSequence.txt";

    private void OnApplicationQuit()
    {
        ////find the correct destination drive
        //string[] drives = Directory.GetLogicalDrives();
        //foreach (string drive in drives)
        //{
        //    if (drive == @"E:\")
        //    {
        //        destinationDrive = drive;
        //        Debug.Log("Found " + drive);
        //    }
        //}

        //string inputPath = "D:\\UnityProjects\\Platformer\\Pyre2D\\Assets\\MyScripts\\";
        //inputPath = Path.GetFullPath(inputPath);
        //inputPath = Path.Combine(inputPath, inputSeq);


        //if (File.Exists(inputPath))
        //{
        //    Debug.Log("You fucking found it");
        //} else
        //{
        //    Debug.Log("back to the drawinf board fuck nugget");
        //}
           
        //destinationDrive += inputSeq;

        //FileUtil.CopyFileOrDirectory(inputPath, destinationDrive);

    }

}


