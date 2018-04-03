/** 

* Turn off the coin and increase score when player
* collides with object

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private PlatformerCharacter2D player;
    public GameObject coin;
    public int value = 0;


    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlatformerCharacter2D>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            player.AddScore(value);
            coin.SetActive(false);
        }
    }

}
