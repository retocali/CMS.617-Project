using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer : MonoBehaviour {

	public GameObject playerPrefab;

	public Transform playerSpawn;

	public CheckpointMaster checkMaster;
	public Color defaultColor;
	
	// Use this for initialization
	void Start () {
		defaultColor = GetComponent<MeshRenderer>().material.color;	
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
			GetComponent<MeshRenderer>().material.color = new Color(1,1,1,0.15f);	
        	checkMaster.ChangePoints(this.gameObject);
		}
	}

	public void DestroySelf() {
		// live = false;
	}

}
