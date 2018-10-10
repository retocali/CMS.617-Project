using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMaster : MonoBehaviour {

	public GameObject[] currentCheckpoint;

	public GameObject[] currentEnemySpawn;

	private GameObject cur;

	private Vector3 pos;

	public GameObject player;

	private GameObject enemyCur;

	private Vector3 enemyPos;

	private GameObject enemy;

	private int index = 0;

	// Use this for initialization
	void Start () {
		if (currentCheckpoint.Length > 0) {
			cur = currentCheckpoint[0];
			pos = cur.transform.position;
		} else {
			Debug.LogWarning("Could not find any checkpoints");
		}

		if (currentEnemySpawn.Length > 0) {
			enemyCur = currentEnemySpawn[0];
			enemyPos = enemyCur.transform.position;
			enemy = enemyCur.GetComponent<EnemySpawnScript>().Spawn();
		} else {
			Debug.LogWarning("Could not find sense of urgency");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerScript>().IsDead() == true) {
			SpawnPlayer();
			if (enemy) {
				Destroy(enemy);
				enemy = enemyCur.GetComponent<EnemySpawnScript>().Spawn();
			}
		}
		if (Input.GetKeyDown("1"))
		{
			index = (index + 1) % currentCheckpoint.Length;
			cur = currentCheckpoint[index];
			pos = currentCheckpoint[index].transform.position;
			Debug.Log(index);
			enemyCur = currentEnemySpawn[index];
			enemyPos = currentEnemySpawn[index].transform.position;
		}
	}

	public void ChangePoints(GameObject newPoint) {
		for (int i = 0; i < currentCheckpoint.Length; i++) {
			if (currentCheckpoint[i].transform.position == newPoint.transform.position) {
				cur = currentCheckpoint[i];
				pos = currentCheckpoint[i].transform.position;
				Debug.Log(cur);
				Debug.Log(pos);
				index = i;
				if (enemy) {
					enemyCur = currentEnemySpawn[index];
					enemyPos = currentEnemySpawn[index].transform.position;
				}
			} 
			else {
				currentCheckpoint[i].GetComponent<MeshRenderer>().material.color =
					currentCheckpoint[i].GetComponent<spawnPlayer>().defaultColor;	
			}
		}
	}

	public void SpawnPlayer() {
		player.GetComponent<PlayerScript>().SpawnPlayer(pos);
	}
}
