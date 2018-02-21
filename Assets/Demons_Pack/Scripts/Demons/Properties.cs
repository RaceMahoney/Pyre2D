using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It is used to configure the demon: head, gun, body and shield.
/// </summary>
public class Properties : MonoBehaviour {
    public int DemonHead = 0;                       //Between 0 - 24
    public int DemonBody = 0;                       //Between 0 - 2
    public int DemonGun = 0;                        //Between 0 - 1
    public int DemonShield = 0;                     //Between 0 - 3
    public GameObject[] Heads;
    public GameObject[] Bodies;
    public GameObject[] Guns;
    public GameObject[] Shields;
    public GameObject dying;

    void Update()
    {
        All_Ok();
    }
    // Use this for initialization
    void Start () {
        All_Ok();
        GetComponent<Animator>().enabled = false;
        Enable_Head();
        Enable_Body();
        Enable_Gun();
        Enable_Shield();
        GetComponent<Animator>().enabled = true;
    }
    /// <summary>
    /// Enable selected head
    /// </summary>
    private void Enable_Head() {
        for (int i = 0; i < Heads.Length; i++) {
            if (i != DemonHead)
            {
                Destroy(Heads[i]);
            }
        }
    }
    /// <summary>
    /// Enable selected body
    /// </summary>
    private void Enable_Body() {
        for (int i = 0; i < Bodies.Length; i++) {
            if (i != DemonBody)
            {
                Destroy(Bodies[i]);
            }
        }
    }
    /// <summary>
    /// Enable selected gun
    /// </summary>
    private void Enable_Gun() {
        for (int i = 0; i < Guns.Length; i++)
        {
            if (i != DemonGun)
            {
                Destroy(Guns[i]);
            }
        }
    }
    /// <summary>
    /// Enable selected shield
    /// </summary>
    private void Enable_Shield() {
        for (int i = 0; i < Shields.Length; i++)
        {
            if (i != DemonShield)
            {
                Destroy(Shields[i]);
            }
        }
    }
    /// <summary>
    /// Correct values?
    /// </summary>
    private void All_Ok() {
        if (DemonHead > 24)
        {
            Debug.Log("--> DemonHead value <color=red>error</color>! \nOnly values between (0-24).");
            DemonHead = Random.Range(0, 24);
        }
        if (DemonBody > 2)
        {
            Debug.Log("--> DemonBody value <color=red>error</color>! Only values between (0-2).");
            DemonBody = Random.Range(0, 2);
        }
        if (DemonGun > 1)
        {
            Debug.Log("--> DemonGun value <color=red>error</color>! Only values between (0-1).");
            DemonGun = Random.Range(0, 1);
        }
        if (DemonShield > 3)
        {
            Debug.Log(DemonShield + "--> DemonShield value <color=red>error</color>! Only values between (0-3).");
            DemonShield = Random.Range(0, 3);
        }
    }
    
}
