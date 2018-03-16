using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets._2D;
//using System.Windows.Forms;



//get connected to player controller
[RequireComponent(typeof(PlatformerCharacter2D))]
public class AutoInput : MonoBehaviour {
    private PlatformerCharacter2D player;
    private PauseMenu menu;

    private static string input;
    private static string destinationDrive;         //eventually set to the destination drive E:\
    private bool m_Jump;
    private bool m_Dash;
    private bool m_Attack;
    private bool m_Pause;
    private int index = 1;
    private float xfloat;
    private float yfloat;
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackCd = 0.1f;
    private Animator m_Anim;
    public float speed = 0.3f;
    private double nextActionTime = 0.0;
    private double peroid = 5.0;

    //the 8 variables to be written to the file and their default values
    private float X = 0f;
    private float Y = 0f;
    private string JUMP = "_";
    private string DASH = "_";
    private string ATTACK = "_";
    public float TIME = 0f;
    private float XPos = 0f;
    private float YPos = 0f;

    private char del = ',';         //delimeter
    private static List<string> input_list; //list of inputs from the file
    public Vector2 targetPos;
    public Collider2D attackTrigger;

    private void Awake()
    {
        
        //find the correct destination drive
        string[] drives = Directory.GetLogicalDrives();
        foreach (string drive in drives)
        {
            if (drive == @"E:\")
            {
                destinationDrive = drive;
                destinationDrive += @"\inputs\inputSequence.txt";
            }
        }

        player = GetComponent<PlatformerCharacter2D>();
        m_Anim = gameObject.GetComponent<Animator>();
        menu = GetComponent<PauseMenu>();
    }

    private void Start()
    {
        attackTrigger.enabled = false;
        input_list = new List<string>();
        ReadString();
        //GetInitalTime();
        //Time.maximumDeltaTime = TIME; //Set the first frame delta time
    }
   

    private void Update()
    {
        
        GetPlayerInput(index);
        Time.fixedDeltaTime = TIME; //update the max time with each new frame
        index++;

   

    }

    private void GetPlayerInput(int index)
    {
        try
        {
            //get the current variable values from the string item in the list based on frame count
            //break that string into its appropriate variables to replicate
            //every frame value from the human player
            string line = input_list[index];
            string[] values = line.Split(new Char[] {',', ',', ',' , ',' , ',' , ',' , ',' }, StringSplitOptions.RemoveEmptyEntries);

            //save current values to variables to check this frame
            X = float.Parse(values[0]);
            Y = float.Parse(values[1]);
            JUMP = values[2];
            DASH = values[3];
            ATTACK = values[4];
            TIME = float.Parse(values[5]);
            XPos = float.Parse(values[6]);
            YPos = float.Parse(values[7]);

            //Set Target Position
            targetPos = new Vector2(XPos, YPos);


            //check for jump and set m_Jump in Platformer2DUserController to true
            if (JUMP.Equals("Jump"))
            {
                m_Jump = true;
            }

            //check for dash and set m_Dash in Platformer2DUserController to true
            if (DASH.Equals("Dash"))
            {
                m_Dash = true;
            }

            //check for pause and set m_Pause in Pausemenu to true
            //if (PAUSE.Equals("Pause"))
            //{
            //    m_Pause = true;
            //}



            //check for if an attack was used
            if (ATTACK.Equals("Attack") && !attacking)
            {
                m_Anim.Play("FireHeroAttack");
                attacking = true;
                attackTimer = attackCd;
                attackTrigger.enabled = true;
                
            }

            if (attacking)
            {
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    attacking = false;
                    attackTrigger.enabled = false;
                }
            }

           

            //since frames will continue after file has ended,
            //out of range exception will occur until game terminates
        }
        catch (ArgumentOutOfRangeException e)
        {
            Application.Quit();
            System.Console.WriteLine(e.Message);
            // Set IndexOutOfRangeException to the new exception's InnerException.
            throw new System.ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
}


    private void FixedUpdate()
    {

        //move the player with the correct recored values
        player.Move(X, m_Jump);
        player.Dash(m_Dash);


        //pause or unpause the game
        //TODO implement the pause into replay
        // menu.paused = m_Pause;

        //check if XPos and YPos are NOT ZERO
        //if so, make a correction
        if (XPos != 0 || YPos != 0)
        {
            player.m_Rigidbody2D.velocity = Vector2.zero;
            player.Correction(targetPos, X);
            updatePos();
        }


        m_Jump = false;
        m_Dash = false;

        //if x seconds have passed
        ////then get the current player position and write it to a file
        //if (Time.time > nextActionTime)
        //{
        //    nextActionTime += peroid;
        //    StartCoroutine(Sync());
        //}

    }
    

    static void ReadString()
    {
        if (File.Exists(destinationDrive))
        {
            //create reader and add each object into list
            StreamReader reader = new StreamReader(destinationDrive);
            for (int i = 0; reader.Peek() > 0; i++)
            {
                input = reader.ReadLine();
                input_list.Add(input);
            }
            reader.Close();
        }
        else
        {
            Debug.Log("Cannot find the file");
        }
       
    }

    private void updatePos()
    {
        //move to correct position  without telemorting
        transform.position = new Vector2(XPos, YPos);
        
    }

    private IEnumerator Sync()
    {
        //turn off the time to catch up
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3);
        updatePos();
        Time.timeScale = 0.1f;
    }

    private void displayStuff()
    {
        //display x
        Debug.Log("Frame:" + Time.frameCount);
        Debug.Log("Current X:" + player.m_Rigidbody2D.velocity.x);
        Debug.Log("Read X:" + X);
    }

}
