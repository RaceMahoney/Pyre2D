using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It is used to instantiate random demons and riders
/// </summary>
public class Instancer : MonoBehaviour {
    public GameObject Demon;                    //  ./ Resources / Demon / Demon
    public GameObject[] Riders;                 //  ./ Resources / Rider / 1,2,3,4
    public GameObject InstancePoint;            //The demons will spawn here
    private bool active = true;                 //to stop making demons set false
	// Use this for initialization
	void Start () {
        StartCoroutine(Create());
	}
    /// <summary>
    /// Create random demon or rider
    /// If demon... it will select random head, body, gun and shield
    /// If rider... it will select between the 4 rider prefabs
    /// Time between demons random between 1f , 3f
    /// </summary>
    /// <returns></returns>
    IEnumerator Create()
    {
        while (active == true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));                          //Time between demons
            int aux = Random.Range(0, 2);                                                   //0 demon 1 rider
            GameObject go;
            switch (aux) {
                case 0:
                    go = Instantiate(Demon, InstancePoint.transform.position, Quaternion.identity) as GameObject;
                    go.name = "Demon";
                    go.GetComponent<Properties>().DemonHead = Random.Range(0,25);
                    go.GetComponent<Properties>().DemonBody = Random.Range(0, 3);
                    go.GetComponent<Properties>().DemonGun = Random.Range(0, 2);
                    go.GetComponent<Properties>().DemonShield = Random.Range(0, 4);
                    break;
                case 1:
                    int number = Random.Range(0, 4);
                    go = Instantiate(Riders[number], InstancePoint.transform.position, Quaternion.identity) as GameObject;
                    go.name = "Rider" + (number+1);
                    break;
            }
        }
    }
}
