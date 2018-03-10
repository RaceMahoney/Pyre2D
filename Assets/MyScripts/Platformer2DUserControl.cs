using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;
using System.Collections;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    private PlatformerCharacter2D m_Character;
    private PlatformerCharacter2D rigidbody;
    private PlayerAttack playerAttack;

    [HideInInspector]
    public bool m_Jump;         //current jump state
    [HideInInspector]
    public bool m_Attack;       //current attack state
    [HideInInspector]
    public bool m_Dash;       //current attack state
   
    private float h = 0f;
  


    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        playerAttack = GetComponent<PlayerAttack>();

       
    }


    private void Update()
    {
        if (m_Character.validInput)
        {
            //get the current float X value
            h = CrossPlatformInputManager.GetAxis("Horizontal");

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_Dash){
                m_Dash = CrossPlatformInputManager.GetButtonDown("Dash");
            }

        }
    }

    private void FixedUpdate()
    {

        if (m_Character.validInput)
        {
          
            m_Character.Move(h, m_Jump);
            m_Character.Dash(m_Dash);
            

            m_Jump = false;
            m_Dash = false;

        }
    }


}

