using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Campfires : MonoBehaviour {

    public GameObject unlit;
    public GameObject lit;

    public Text Thanks;

    private PlatformerCharacter2D m_Character;
    private Rigidbody2D m_Rigidbody2D;

    // Use this for initialization
    void Start () {
        lit.SetActive(false);

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
