using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScenePosWrap : MonoBehaviour {

    public GameObject player;

    public float leftPos;
    public float rightPos;
    public float offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player.transform.position.x > rightPos)
        {
            player.transform.position -= new Vector3(offset, 0, 0);
        }
        if (player.transform.position.x < leftPos)
        {
            player.transform.position += new Vector3(offset, 0, 0);
        }
		
	}
}
