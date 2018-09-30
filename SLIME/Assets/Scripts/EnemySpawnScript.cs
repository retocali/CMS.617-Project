using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour {


	public GameObject enemyPrefab;

	public Transform enemySpawn;

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
		var enemy = (GameObject)Instantiate (
			enemyPrefab,
			this.transform.position,
			this.transform.rotation);

		return enemy;

	}


}
