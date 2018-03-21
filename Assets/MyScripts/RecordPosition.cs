using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RecordPosition : MonoBehaviour {

    //private Timeline_Transform timeline;
    private List<string> posTime;
    private string[] line = { " " };
    private string destinationDrive;
    private string file;
    private PlayerAttack playerAttack;
    private ScreenRecorder screenRecorder;
    private bool isReplay = false;

  

    // Use this for initialization
    void Start () {

        File.WriteAllLines("Assets/MyScripts/positions.txt", line);
        playerAttack = GetComponent<PlayerAttack>();
        screenRecorder = GetComponent<ScreenRecorder>();
        destinationDrive = Application.dataPath + "/MyScripts/positions.txt";

    }

    private void Update()
    {
        
        if (!isReplay)
        {
            //will only allow recording to happen when not in replay mode
            if (CrossPlatformInputManager.GetButtonUp("FireAttack") && !playerAttack.attacking)
            {
                //if player is attacking, rememeber that position
                //so that when the player reaches it again he attacks again
                WritePosAttack(GetPos());
            }

        }
       
    }

    private Vector3 GetPos()
    {
        Vector3 currentPos = transform.position;
        return currentPos;
    }



    void WritePosAttack(Vector3 pos)
    {
        playerAttack.vectors.Add(pos);
    }


    public void SetReplay()
    {
        isReplay = true;
    }

}
