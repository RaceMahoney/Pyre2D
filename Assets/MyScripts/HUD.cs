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

        private PlatformerCharacter2D m_Character;

        private void Start()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            HeartUI.sprite = HeartSrpites[m_Character.health];
        }

    }
}
