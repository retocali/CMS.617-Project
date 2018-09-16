using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPlayer : MonoBehaviour {

	public GameObject playerPrefab;

	public Transform playerSpawn;

	// Use this for initialization
	void Start () {
		Spawn();
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount < 1){
			Spawn();

		}
		
	}

	void Spawn() {
		var player = (GameObject)Instantiate (
			playerPrefab,
			playerSpawn.position,
			playerSpawn.rotation);

		player.transform.parent = gameObject.transform;
	}
}
