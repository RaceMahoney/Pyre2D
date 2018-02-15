using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityStandardAssets._2D;


//get connected to player controller
[RequireComponent(typeof(PlatformerCharacter2D))]
public class AutoInput : MonoBehaviour {
    private PlatformerCharacter2D player;

    private static string input;
    private static string path = "Assets/MyScripts/inputSequence.txt";
    private bool m_Jump;
    private bool m_Dash;
    private bool m_Attack;
    private int index = 0;
    private float xfloat;
    private float yfloat;
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackCd = 0.1f;
    private Animator m_Anim;
    public float speed = 0.3f;

    //the 5 variables to be written to the file and their default values
    private float X = 0f;
    private float Y = 0f;
    private string JUMP = "_";
    private string DASH = "_";
    private string ATTACK = "_";

    //delemeter
    private char del = ',';


    private static List<string> input_list;

    public Collider2D attackTrigger;
    
    private void Start()
    {
        player = GetComponent<PlatformerCharacter2D>();
        m_Anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
        input_list = new List<string>();
        ReadString();
        
    }

    private void FixedUpdate()
    {

        //TODO: is off by an average if 14 frames, need to find solution
        try
        {
            //get the current variable values from the string item in the list based on frame count
            //break that string into its appropriate variables to replicate
            //every frame value from the human player
            string line = input_list[index];
            string[] values = line.Split(new Char[] { ',', ',', ',', ',' }, StringSplitOptions.RemoveEmptyEntries);

            //save current values to variables to check this frame
            X = float.Parse(values[0]);
            Y = float.Parse(values[1]);
            JUMP = values[2];
            DASH = values[3];
            ATTACK = values[4];

            //check each frame with the current line to know
            //what to do that frame


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


            //check for if an attack was used
            if (ATTACK.Equals("Attack") && !attacking)
            {
                m_Anim.Play("FireHeroAttack");
                attacking = true;
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
        }
        //since frames will continue after file has ended,
        //out of range exception will occur until game terminates
        catch (ArgumentOutOfRangeException e)
        {
            Application.Quit();
            System.Console.WriteLine(e.Message);
            // Set IndexOutOfRangeException to the new exception's InnerException.
            throw new System.ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        
 
        //TODO remove all trace of crouch option from code
        bool crouch = false;

        player.Move(X, crouch, m_Jump, m_Dash);
        m_Jump = false;
        m_Dash = false;

        //stop the index from incremdening if there is no more 
        //item in the list
        //TODO make game stop when there are no more inputs
        if (index == input_list.Count)
        {
            Application.Quit();
        }

        index++;
                
    }
    

    static void ReadString()
    {
        //create reader and add each object into list
        StreamReader reader = new StreamReader(path);
        for(int i = 0; reader.Peek() > 0; i++)
        {
            input = reader.ReadLine();
            input_list.Add(input);
                
        }
        reader.Close();
    }

}
