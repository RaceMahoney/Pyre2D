using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class RecordInputs : MonoBehaviour{

    private Platformer2DUserControl controller;

    private float inputX;
    private float inputY;
    private string directionVal;

    private void Start()
    {
        Platformer2DUserControl controller = GetComponent<Platformer2DUserControl>();
        //clear the file
        //TODO find a better solution instead of deleting the file
        //Idea: have the player tester enter their name so that 
        //the file is saved unquiely
        File.Delete("Assets/MyScripts/inputSequence.txt");
       
    }

    private void Update()
    {
       
        //get direction input and save x and y values as a float
        Vector2 directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));     
        inputX = directionalInput.x;
        inputY = directionalInput.y;

       // Debug.Log("Recorded input x " + inputX);
        //Debug.Log("Recorded input y " + inputY);

        
        //if there is no X or Y direction then write o
        if(inputX == 0 && inputY == 0)
        {
            WriteString("0");
        }
     
        //if the player moves left or right
        if(inputX > 0 ||inputX < 0)
        {
            //conver float to string and write to file
            directionVal = inputX.ToString();
            WriteString("X." + directionVal);
        }

        //if the player moves up or down
        if (inputY > 0 || inputY < 0)
        {
            //conver float to strinf and write to file
            directionVal = inputY.ToString();
            WriteString("Y." + directionVal);
        }
        

        //check if normal for double jump
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            WriteString("Jump");
        }

        if (CrossPlatformInputManager.GetButtonUp("Dash"))
        {
            WriteString("Dash");
        }
        if (CrossPlatformInputManager.GetButtonUp("FireAttack"))
        {
            WriteString("Attack");
        }



    }

    static void WriteString(string input)
    {
        //create writer object & write to file
        string file = "Assets/MyScripts/inputSequence.txt";
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(input);
        writer.Close();

    }

}
