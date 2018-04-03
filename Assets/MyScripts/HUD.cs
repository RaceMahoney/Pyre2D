/** 

* This script controls the HUD overlay of the game.
* Inclduing health hearts, dash status and score. 

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets._2D
{

    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class HUD : MonoBehaviour {
        public Sprite[] HeartSrpites;           
        public Sprite[] DashSprites;
        public Image DashUI;
        public Image HeartUI;
        public Text scoreText;
        public Text ready;
        private PlatformerCharacter2D m_Character;

        private void Start()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
            SetText();
        }

        private void Update()
        {
            try
            {
                //update the number of hearts
                HeartUI.sprite = HeartSrpites[m_Character.health];

            } catch (IndexOutOfRangeException e)
            {
                //if array goes out of range default to full
                HeartUI.sprite = HeartSrpites[0];
            }
            //update the amount of coins
            SetText();
            //update dash
            DashUI.sprite = DashSprites[m_Character.dashStatus];
            
            if(m_Character.dashStatus == 0)
            {
                ready.enabled = true;
            } else
            {
                ready.enabled = false;
            }


        }

        private void SetText()
        {
            //upate the current amount of coins collected
            scoreText.text = "Coins: " + m_Character.score.ToString();
        }

    



    }
}
