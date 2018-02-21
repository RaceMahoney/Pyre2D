using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// It is used to autodestroy prefabs
/// </summary>
public class AutoDestruction : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("Destroy_", 0.5f);
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.gameObject.name == "Magic"&&(other.tag == "Demon" || other.tag == "Rider"))
        {
            GetComponent<Collider2D>().enabled = false;
        }
        if (this.gameObject.name == "Damage"&&other.name=="Golem")
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }

    void Destroy_() {
        Destroy(this.gameObject);
    }
}
