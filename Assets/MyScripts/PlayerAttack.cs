using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

 
    private bool attacking = false;
    private float attackTimer = 0;
    private float attackCd = 0.1f;

    public Collider2D attackTrigger;
    private Animator m_Anim;

    private void Awake()
    {
        m_Anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
    }

    private void Update()
    {
       
        if(CrossPlatformInputManager.GetButtonUp("FireAttack") && !attacking)
        {
            m_Anim.Play("FireHeroAttack");
            attacking = true;
            attackTimer = attackCd;
            attackTrigger.enabled = true;
        }

        if (attacking)
        {
            
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            } else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
            

            
        }

        
    }

}
