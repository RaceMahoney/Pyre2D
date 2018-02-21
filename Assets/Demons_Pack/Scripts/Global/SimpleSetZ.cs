using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSetZ : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
