using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SrirachaHealth : MonoBehaviour {

    private PlatformerCharacter2D player;
    public GameObject bottle;

	// Use this for initialization
	void Start () {



    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            player = other.GetComponent<PlatformerCharacter2D>();
            if(player != null)
            {
                //check current health
                if (player.health < 5)
                {
                    player.health += 3;

                    //make sure it did not go over 5
                    if (player.health > 5)
                    {
                        //reset back to 5
                        player.health = 5;
                    }
                }
                ///TODO play a sound effect
                bottle.SetActive(false);
            }
            
        }
    }
}
