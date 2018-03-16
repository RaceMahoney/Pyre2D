using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EnemyRecorder : MonoBehaviour {

    public GameObject[] enemies;
    private string[] line = { " " };
    private string destinationDrive;

    public GameObject testEnemy;

    //X and Y position of the enemy
    private float Xpos = 0f;

    // Use this for initialization
    void Start () {
        enemies = GameObject.FindGameObjectsWithTag("Demon");
        

        //find the correct destination drive
        string[] drives = Directory.GetLogicalDrives();
        foreach (string drive in drives)
        {
            if (drive == @"E:\")
            {
                destinationDrive = drive;
                destinationDrive += @"\inputs\enemySequence.txt";
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        Xpos = testEnemy.transform.position.x;
        WritePos(Xpos);

    }

    void WritePos(float w_X)
    {
        string currentPos = w_X + " ";
        string file = "Assets/MyScripts/enemySequence.txt";
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(currentPos);
        writer.Close();
    }

    private void WriteBreak()
    {
        string breakline = "~";
        string file = "Assets/MyScripts/enemySequence.txt";
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(breakline);
        writer.Close();
    }

    private IEnumerator RecordTime(GameObject badGuy)
    {
        yield return new WaitForSeconds(2);
        Xpos = badGuy.transform.position.x;
        WritePos(Xpos);
    }
}
