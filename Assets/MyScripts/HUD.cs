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
            //update the number of hearts
            HeartUI.sprite = HeartSrpites[m_Character.health];
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
            scoreText.text = "Coins: " + m_Character.score.ToString();
        }

    



    }
}
