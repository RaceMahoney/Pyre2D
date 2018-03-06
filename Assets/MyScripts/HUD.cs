using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets._2D
{

    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class HUD : MonoBehaviour {
        public Sprite[] HeartSrpites;
        public Image HeartUI;
        public Text scoreText;
        public Text Thanks;
        private static bool end;

        private PlatformerCharacter2D m_Character;
        private Campfires campfire;

        private void Start()
        {
            if (Thanks.enabled)
            {
                Thanks.enabled = false;
            }
            m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
            campfire = GetComponent<Campfires>();
            end = campfire.end;
            SetText();
        }

        private void Update()
        {
           // end = campfire.end;
            HeartUI.sprite = HeartSrpites[m_Character.health];
            SetText();

            if (end == true)
            {
                Thanks.enabled = true;
            }

            
        }

        private void SetText()
        {
            scoreText.text = "Coins: " + m_Character.score.ToString();
        }

     

    }
}
