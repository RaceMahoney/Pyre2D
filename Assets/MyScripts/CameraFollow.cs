/** 

* This script smooths the camera movement following
* the player and sets boundaries within the world
* space of the game so that camera does not 
* venture to unwanted areas

* @author Race Mahoney
* @data 04/02/18
* @framework .NET 3.5

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Vector2 velocity;               //how fast the camera is moving

    public float smoothTimeY;               //smoothing X value
    public float smoothTimeX;               //smoothing Y value

    public GameObject player;               //refrence to player 

    public bool bounds;                     //set where the bounds of the camera are

    public Vector3 minCameraPos;
    public Vector3 maxCameraPos;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}

    private void FixedUpdate()
    {
        //every frame adjust the position of the camera by smoothing that movement based on the player's position
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        //set a bound position form the Editor
        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
                                             Mathf.Clamp(transform.position.y, minCameraPos.y, maxCameraPos.y),
                                             Mathf.Clamp(transform.position.z, minCameraPos.z, maxCameraPos.z));
        }
    }

    public void SetMinCamPosition()
    {
        minCameraPos = gameObject.transform.position;
    }

    public void SetMaxCamPosition()
    {
        maxCameraPos = gameObject.transform.position;
    }

}
