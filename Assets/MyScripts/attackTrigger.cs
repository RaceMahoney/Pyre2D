using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour {

    public int dmg = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.isTrigger != true && collision.CompareTag("Demon"))
        {
            collision.SendMessageUpwards("Damage", dmg);
        }
    }
}
