/** 

* Script activates when player attacks

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour {

    public int dmg = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.isTrigger != true && collision.CompareTag("Demon"))
        {
            // Once a collison between the attack box and the demon collider, send damange value to parent script
            collision.SendMessageUpwards("Damage", dmg);
        }
    }
}
