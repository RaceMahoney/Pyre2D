using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class RecordInputs : MonoBehaviour{

    private Platformer2DUserControl controller;
    private PlatformerCharacter2D character;

    private float inputX;
    private float inputY;
    private string directionVal;

    //the 5 variables to be written to the file and their default values
    private float X = 0f;
    private float Y = 0f;
    private string JUMP = "_";
    private string DASH = "_";
    private string ATTACK = "_";

    public int timebetween = 300; //milliseconds


    private void Start()
    {
        Platformer2DUserControl controller = GetComponent<Platformer2DUserControl>();
        PlatformerCharacter2D character = GetComponent<PlatformerCharacter2D>();
        //clear the file
        //TODO find a better solution instead of deleting the file
        //Idea: have the player tester enter their name so that 
        //the file is saved unquiely
        File.Delete("Assets/MyScripts/inputSequence.txt");
      
    }

    private void Update()
    {
        //reset the bools
        JUMP = "_";
        DASH = "_";
        ATTACK = "_";

        //get direction input and save x and y values as a float
        Vector2 directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputX = directionalInput.x;
        //TODO: Get Y Velocity to write to file
        inputY = directionalInput.y;

        // Debug.Log("Recorded input x " + inputX);
        //Debug.Log("Recorded input y " + inputY);


        //if there is no X or Y direction then write o
        if (inputX == 0 && inputY == 0)
        {
            //reset variables back to default
            X = 0f;
            Y = 0f;
        }

        //if the player moves left or right
        if (inputX > 0 || inputX < 0)
        {
            //save the current player's x-axis speed in X;
            X = inputX;
        }

        //if the player moves up or down
        if (inputY > 0 || inputY < 0)
        {
            //save the current player's y-axis speed in Y;
            Y = inputY;
        }


        //check if normal for double jump
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            //record that jump was pressed this frame
            JUMP = "Jump";
        }

        //TODO make a dash start and dash end
        if (CrossPlatformInputManager.GetButtonDown("Dash"))
        {
            //record that dash was pressed this frame
            DASH = "Dash";
        }
        if (CrossPlatformInputManager.GetButtonDown("FireAttack"))
        {
            //record that attack was pressed this frame
            ATTACK = "Attack";
        }




        //write the current values at the end of every frame
        //if there has been a change
        //if (X != 0f || Y != 0f || JUMP != "_" || DASH != "_" || ATTACK != "_")
        WriteString(X, Y, JUMP, DASH, ATTACK);

    }
    

    static void WriteString(float w_X, float w_Y, string w_Jump, string w_Dash, string w_Attack)
    {
        //combine all values for this frame into one string
        string input = w_X + "," + w_Y + "," + w_Jump + "," + w_Dash + "," + w_Attack + "," + Time.deltaTime +",";
        //create writer object & write to file
        string file = "Assets/MyScripts/inputSequence.txt";
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(input + "\t\t\t\tRECORDED AT FRAME:" + Time.frameCount);
        writer.Close();

    }

    static void Reset()
    {

    }

}
