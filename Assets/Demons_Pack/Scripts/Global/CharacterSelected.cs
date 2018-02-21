using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// You can control one demon at the same time.
/// You can select the demon with the mouse onlick
/// </summary>
public class CharacterSelected : MonoBehaviour {
    public GameObject selected;                     //Selected demon
    private GameObject[] demons;                    //All demons into the scene
    private GameObject[] riders;                    //All pet riders into the scene
    // Use this for initialization
    void Start () {
        selected = null;
	}
    /// <summary>
    /// Set the selected demon
    /// </summary>
    /// <param name="go">demon</param>
    public void Set(GameObject go)
    {
        if (selected != null)
        {
            if (selected != go)
            {
                selected.SendMessage("SetAuto");
            }
        }
        selected = go;
        demons = null;
        demons = GameObject.FindGameObjectsWithTag("Demon");
        foreach (GameObject demon in demons)
        {
            if (demon.GetComponent<Demon_Controller>().auto == false && demon != selected)
            {
                demon.SendMessage("SetAuto");
            }
        }
        riders = null;
        riders = GameObject.FindGameObjectsWithTag("Rider");
        foreach (GameObject rider in riders)
        {
            if (rider.GetComponent<Rider_Controller>().auto == false && rider != selected)
            {
                rider.SendMessage("SetAuto");
            }
        }        
    }
}
