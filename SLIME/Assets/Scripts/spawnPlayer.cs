using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer : MonoBehaviour {

	public GameObject playerPrefab;

	public Transform playerSpawn;

	public CheckpointMaster checkMaster;

	// Use this for initialization
	void Start () {
		
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
		if(collider.tag == "Player") {
		Debug.Log("YO");
		Debug.Log(checkMaster);
        checkMaster.ChangePoints(this.gameObject);
		}
		
    }

	public void DestroySelf() {
		// live = false;
	}

}
