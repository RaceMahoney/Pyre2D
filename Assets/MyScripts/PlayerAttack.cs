/** 

* This script controls the attacking action of the player.
* Manages the animation time of the attack animation.


* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using System.IO;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour {

    [HideInInspector]
    public bool attacking = false;          //determines if the player is attacking
    private float attackTimer = 0;          //timer
    private float attackCd = 0.1f;          //length of animation

    public Collider2D attackTrigger;        //collider trigger
    private Animator m_Anim;

    [HideInInspector]
    public List<Vector3> vectors;           //vectors to be reached in replay mode
    private bool isReplay = false;          //determines if in replay mode

    private void Awake()
    {
        m_Anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
    }

    private void Update()
    {
        //Attack key is hit and not already attacking
        if(CrossPlatformInputManager.GetButtonUp("FireAttack") && !attacking)
            StartAttack();

        //check to see when attacking should be turned off
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

        if (isReplay)
        {
            AutoAttack();
           
        }
    }

    public void StartAttack()
    {
        //begin process of attacking
        if (!attacking)
        {
            m_Anim.Play("FireHeroAttack");
            attacking = true;
            attackTimer = attackCd;
            attackTrigger.enabled = true;
        }
    }

    public void AutoAttack()
    {
        //when player position is equal to recored transfrom
        //call StartAttack()
        try
        {
            foreach (Vector3 vect in vectors)
            {
                float dist = Vector3.Distance(transform.position, vect);
                //transform string to vector
                if (dist > 0 && dist < 0.5f)
                {            
                    //remove this vector so it is not triggered again
                    vectors.Remove(vect);
                    StartAttack();

                }
            }
        } catch (InvalidOperationException e)
        {
           
        }
        

    }

    public void SetReplay()
    {
        isReplay = true;
    }

}
