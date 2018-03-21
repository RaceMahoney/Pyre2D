using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SceneReset : MonoBehaviour {
    public GameObject[] coins;
    public GameObject[] enemies;
    public GameObject[] bottles;
    public GameObject[] unlit;
    public GameObject[] lit;
    public Text Thankyou;

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

        if (Thankyou.IsActive())
        {
            Thankyou.enabled = false;
        }
    }
}
