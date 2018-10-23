using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SplitterScript : MonoBehaviour, ToolsInterface {

	private bool split = false;
 	private GameObject player;

	private GameObject player2;

 	 private float y; 
 	 private float x;
 	// Use this for initialization
	void Start () {
		y = gameObject.transform.position.y;
		x = gameObject.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (player2 != null) {
			if ( player2.GetComponent<PlayerScript>().IsDead()) {
				resetSplitter();
			}
		}


	}
	public void Interact(GameObject p)
	{
		if (!split) {
			player = p;
			p.transform.position = new Vector3(x - 1, y, 0);
			player.GetComponent<PlayerScript>().MultiplyVelocity(0);
			player2 = Instantiate(p, new Vector3(x + 1, y, 0), Quaternion.identity);
			player2.GetComponent<PlayerScript>().duplicateSlime();
			split = true;
		}

		
	}


	public void resetSplitter() {
		// player2.GetComponent<PlayerScript>().KillPlayer();
		Destroy(player2);
		split = false;

	}

	public bool isSplit() {
		return split;
	}
}

