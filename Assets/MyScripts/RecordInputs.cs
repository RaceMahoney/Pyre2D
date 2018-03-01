using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
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

    //X and Y position of the player
    private float Xpos = 0f;
    private float Ypos = 0f;

    private double nextActionTime = 0.0;
    public double peroid = 0.4;

    int count;
    byte[] byteArray;
    char[] charArray;
    UnicodeEncoding uniEncoding = new UnicodeEncoding();

    private string[] line = {" "};

    private void Start()
    {
        Platformer2DUserControl controller = GetComponent<Platformer2DUserControl>();
        PlatformerCharacter2D character = GetComponent<PlatformerCharacter2D>();
        //clear the file
        //TODO find a better solution instead of deleting the file
        //Idea: have the player tester enter their name so that 
        //the file is saved unquiely
        File.WriteAllLines("Assets/MyScripts/inputSequence.txt", null);
        //File.Delete("Assets/MyScripts/inputSequence.txt");
    }

    private void Update()
    {
        //reset the bools
        JUMP = "_";
        DASH = "_";
        ATTACK = "_";

        //reset the Pos
        Xpos = 0f;
        Ypos = 0f;

        
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

        //if x seconds have passed
        //then get the current player position and write it to a file
        if(Time.time > nextActionTime)
        {
            nextActionTime += peroid;
            GetPlayerPos();
        }

        //write the current values at the end of every frame
        //if there has been a change
        //if (X != 0f || Y != 0f || JUMP != "_" || DASH != "_" || ATTACK != "_")
        WriteInputs(X, Y, JUMP, DASH, ATTACK, Xpos, Ypos);
 
    }

    void GetPlayerPos()
    {
        Xpos = transform.position.x;
        Ypos = transform.position.y;

    }

    void WriteInputs(float w_X, float w_Y, string w_Jump, string w_Dash, string w_Attack, double w_XPos, double w_YPos)
    {
        int frames = Time.frameCount;
        frames--;//adjust for list starting at 0
        //combine all values for this frame into one string
        string input = w_X + "," + w_Y + "," + w_Jump + "," + w_Dash + "," + w_Attack + "," + Time.deltaTime +"," + w_XPos + "," + w_YPos + ",";


        //create writer object & write to file
        //also wirte to file to compare output
        string file = "Assets/MyScripts/inputSequence.txt";
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(input + "\t\t\t\tRECORDED AT FRAME:" + frames);
        writer.Close();

    }

}
