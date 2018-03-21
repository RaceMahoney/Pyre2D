using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using System.IO;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour {

    [HideInInspector]
    public bool attacking = false;
    private float attackTimer = 0;
    private float attackCd = 0.1f;

    public Collider2D attackTrigger;
    private Animator m_Anim;

    private string destinationDrive;
    [HideInInspector]
    public List<Vector3> vectors;
    private bool isReplay = false;

    private void Awake()
    {
        m_Anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
        destinationDrive = Application.dataPath + "/MyScripts/positions.txt";
    }

    private void Update()
    {
        if(CrossPlatformInputManager.GetButtonUp("FireAttack") && !attacking)
            StartAttack();

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
                    //made it to this vector
                    Debug.Log("Made it to the target " + vect);
                    //remove this vector so it is not triggered again
                    vectors.Remove(vect);
                    StartAttack();

                }
            }
        } catch (InvalidOperationException e)
        {
            Debug.Log("List of attack points are now empty");
        }
        

    }

    public void SetReplay()
    {
        isReplay = true;
    }


    //public void ReadFile()
    //{
    //    if (File.Exists(destinationDrive))
    //    {
    //        string target;
    //        //create reader and add each object into list
    //        StreamReader reader = new StreamReader(destinationDrive);
    //        for (int i = 0; reader.Peek() > 0; i++)
    //        {
    //            target = reader.ReadLine();
    //            vectors.Add(target);
    //        }
    //        reader.Close();
    //    }
    //    else
    //    {
    //        Debug.Log("Cannot find the file");
    //    }
    //    //set isReplay to true to let script know to look for values 
    //    isReplay = true;
    //}

    

}
