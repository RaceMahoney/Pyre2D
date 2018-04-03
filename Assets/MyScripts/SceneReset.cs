/** 

* This script controls the process of reseting the game 
* when replay mode was entered


* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine;

public class SceneReset : MonoBehaviour {
    public GameObject[] coins;
    public GameObject[] enemies;
    public GameObject[] bottles;
    public GameObject[] unlit;
    public GameObject[] lit;
    public GameObject[] backgrounds;
    //public TilemapRenderer rend;
    public Text Thankyou;
    public Canvas ReplayCanvas;
    public GameObject DeathScreen;

    private System.Random rand;
    private Vector3 bottleStartPos;


    public void Reset()
    {
        foreach (GameObject enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
                enemy.SetActive(true);
        }

        foreach (GameObject coin in coins)
        {
            if (!coin.activeInHierarchy)
                coin.SetActive(true);
        }

        foreach (GameObject bottle in bottles)
        {
            if (!bottle.activeInHierarchy) 
                bottle.SetActive(true);


        }

        foreach (GameObject UNlitFire in unlit)
        {
            if (!UNlitFire.activeInHierarchy)
                UNlitFire.SetActive(true);     
        }

        foreach (GameObject litFire in lit)
        {
            if (litFire.activeInHierarchy)
            {
                litFire.SetActive(false);
            }
        }

        foreach (GameObject mountain in backgrounds)
        {
            if (!mountain.activeInHierarchy)
            {
                mountain.SetActive(true);
            }
        }

        DeathScreen.SetActive(false);

        if (Thankyou.IsActive())
        {
            Thankyou.enabled = false;
        }

        ReplayCanvas.renderMode = RenderMode.ScreenSpaceCamera;

    }
}
