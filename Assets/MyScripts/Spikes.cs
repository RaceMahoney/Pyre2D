using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    private PlatformerCharacter2D player;

    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            player = other.GetComponent<PlatformerCharacter2D>();
            if (player != null)
            {
                player.health = 0;
                //TODO play death animation and sound
            }

        }
    }
}
