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
    private int index;
    private float xfloat;
    private float yfloat;
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackCd = 0.1f;
    

    private static List<string> input_list;

    public Collider2D attackTrigger;
    private Animator m_Anim;
    public float speed = 0.3f;


    private void Start()
    {
        player = GetComponent<PlatformerCharacter2D>();
        m_Anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
        input_list = new List<string>();
        ReadString();
        
    }

    private void Update()
    {
        //check each frame with the current line to know
        //what to do that frame
        try
        {
            //check for jump and set m_Jump in Platformer2DUserController to true
            if (input_list[index].Equals("Jump"))
            {
                m_Jump = true;
            }

            //check for dash and set m_Dash in Platformer2DUserController to true
            if (input_list[index].Equals("Dash"))
            {
                m_Dash = true;
            }

            //check for if an attack was used
            if (input_list[index].Equals("Attack") && !attacking)
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
        catch (System.IndexOutOfRangeException e)  
        {
            //TODO end gameplay
            System.Console.WriteLine(e.Message);
            // Set IndexOutOfRangeException to the new exception's InnerException.
            throw new System.ArgumentOutOfRangeException("index parameter is out of range.", e);
        }
        index++;

    }

    private void FixedUpdate()
    {

        xfloat = 0f;
        if ((input_list[index].StartsWith("X")))
        {
            string input = input_list[index];
            string newInput = input.Substring(2);
            xfloat = float.Parse(newInput);

        }
        if ((input_list[index].StartsWith("Y")))
        {
            string input = input_list[index];
            string newInput = input.Substring(2);
            yfloat = float.Parse(newInput);

        }
            

        bool crouch = false;
        Debug.Log(xfloat);
        player.Move(xfloat, crouch, m_Jump, m_Dash);
        m_Jump = false;
        m_Dash = false;



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

    static float ReadFloat()
    {
        float fval = 0f;
        //create reader and return the value of direction
        StreamReader reader = new StreamReader(path);
        for (int i = 0; reader.Peek() > 0; i++)
        {
            input = reader.ReadLine();
            if (input.StartsWith("X") || input.StartsWith("Y"))
            {
                string newSTR = input.Substring(2);
               // fval = float.Parse(input);
                Debug.Log("Here is the new float: " + newSTR);
            }
            
        }

        return fval;
    }

}
