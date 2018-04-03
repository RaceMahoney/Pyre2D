/** 

* This script controls the campfire checkpoints 
* when the player passes by. 

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Campfires : MonoBehaviour {

    public GameObject unlit;        //refrence to unlit campfire object
    public GameObject lit;          //refrence to animated lit campfire object

    public Text Thanks;

    private PlatformerCharacter2D m_Character;
    private Rigidbody2D m_Rigidbody2D;

    // Use this for initialization
    public void Start () {


        if (Thanks.enabled)
        {
            Thanks.enabled = false;
        }

        m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        m_Rigidbody2D = m_Character.GetComponent<Rigidbody2D>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {   
            //turn the campfire on and turn off the unlit campfire
            lit.SetActive(true);
            unlit.SetActive(false);

            if(gameObject.tag == "torch")
            {
                Thanks.enabled = true;
                m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            }

        }
    }


}
