using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfires : MonoBehaviour {

    public GameObject unlit;
    public GameObject lit;

    public bool end = false;

	// Use this for initialization
	void Start () {
        lit.SetActive(false);
       
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            lit.SetActive(true);
            unlit.SetActive(false);

            if(gameObject.tag == "torch")
            {
                end = true;
            }

        }
    }

}
