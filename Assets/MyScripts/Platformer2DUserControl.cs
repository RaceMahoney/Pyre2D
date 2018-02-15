using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;


[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private PlayerAttack playerAttack;

    [HideInInspector]
    public bool m_Jump;
    [HideInInspector]
    public bool m_Dash;
    [HideInInspector]
    public bool m_Attack;

    private long pressTime = 0;
    private long coolTime = 0;
    private bool dashReady = true;
    private float temp = 0f;
    Stopwatch stopWatch = new Stopwatch();
    Stopwatch coolDown = new Stopwatch();


    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }


    private void Update()
    {
        if (m_Character.validInput)
        {

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_Dash)
            {
                //read the dash input in Update so butten presses aren't missed
                if (CrossPlatformInputManager.GetButton("Dash") && dashReady)
                {
                    stopWatch.Start();
                    m_Dash = true;
                    pressTime = stopWatch.ElapsedMilliseconds;


                    if (pressTime >= 200)
                    {
                        m_Dash = false;
                        dashReady = false;
                    }

                }
            }

            if (CrossPlatformInputManager.GetButtonUp("Dash"))
            {
                //stop the watch
                stopWatch.Stop();
                stopWatch.Reset();

                //start the cool down watch
                coolDown.Start();
                coolTime = coolDown.ElapsedMilliseconds;
                if (coolTime >= 3000)
                {
                    dashReady = true;
                    coolDown.Stop();
                    coolDown.Reset();
                }

            }
        }
    }

    private void FixedUpdate()
    {
        if (m_Character.validInput)
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            //TODO make speed more uniform so it is easier to replicate
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump, m_Dash);
            //UnityEngine.Debug.Log("Speed it: " + h);
            
            m_Jump = false;
            m_Dash = false;

        }

    }
}

