using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer : MonoBehaviour {

	public GameObject playerPrefab;

	public Transform playerSpawn;

	public CheckpointMaster checkMaster;
	public Color defaultColor;
	public Color activeColor;
	
	// Use this for initialization
	void Start () {
		defaultColor = GetComponent<MeshRenderer>().material.color;
		activeColor = new Color(128,128,128,128);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (checkMaster == null) {
			checkMaster = GameObject.FindGameObjectsWithTag("GameMaster")[0].GetComponent<CheckpointMaster>();
		}
		
	}

	public GameObject Spawn() {
		var player = (GameObject)Instantiate (
			playerPrefab,
			this.transform.position,
			this.transform.rotation);

		return player;

	}

	

		void OnTriggerEnter2D(Collider2D collider)
    {
		ActivatePoint(collider);
		
    }
	void OnTriggerStay2D(Collider2D collider)
    {
		ActivatePoint(collider);
		
    }

	void OnTriggerExit2D(Collider2D collider)
    {
		ActivatePoint(collider);
    }

	private void ActivatePoint(Collider2D collider)
	{	
		if(collider.tag == "Player") {
			GetComponent<MeshRenderer>().material.color = activeColor;
        	checkMaster.ChangePoints(this.gameObject);
		}
	}

	public void DestroySelf() {
		// live = false;
	}

}
