using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMaster : MonoBehaviour {

	public GameObject[] currentCheckpoint;

	public GameObject[] currentUrgencySpawn;

	private GameObject cur;

	private Vector3 pos;

	public GameObject player;

	private GameObject urgencyCur;

	private Vector3 urgencyPos;

	public GameObject urgency;

	public bool useSpawns;

	private int index = 0;

	private bool checkpointActivated = false;

	private Vector3 lastUrgencyPos;

	//////////////////////////////////////
	// PUBLIC METHODS
	//////////////////////////////////////

	// Tells the gamemaster that the urgency has been changed outside
	// of this file.
	public void urgencyStateChange() {
		checkpointActivated = false;
	}

	// Returns the urgency transform at the current time.
	public Vector3 getLastUrgencyPostion() {
		return  lastUrgencyPos;
	}

	// Checks whether or not the player just recently activated a new checkpoint
	public bool newCheckpointActivated() {
		lastUrgencyPos = urgency.transform.position;
		return checkpointActivated;
	}

	////////////////////////////////////////////////
	// Private Methods
	///////////////////////////////////////////////

	// Use this for initialization
	void Start () {
		urgencyPos = urgencyCur.transform.position;
		if (currentCheckpoint.Length > 0) {
			cur = currentCheckpoint[0];
			pos = cur.transform.position;
		} else {
			Debug.LogWarning("Could not find any checkpoints");
		}

		if (urgency && useSpawns) {
			if (currentUrgencySpawn.Length > 0) {
				urgencyCur = currentUrgencySpawn[0];
				urgencyPos = urgencyCur.transform.position;
				urgency = urgencyCur.GetComponent<EnemySpawnScript>().Spawn();
			} else {
				Debug.LogWarning("Could not find sense of urgency");
			}
		}
		if (urgency && !useSpawns) {
			lastUrgencyPos = urgency.transform.position;
		}
		if (!urgency && !useSpawns) {
			Debug.LogWarning("No Urgency Being Used");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerScript>().IsDead() == true) {
			SpawnPlayer();
			if (urgency && useSpawns) {
				Destroy(urgency);
				urgency = urgencyCur.GetComponent<EnemySpawnScript>().Spawn();
			}
		}
		if (Input.GetKeyDown("1"))
		{
			index = (index + 1) % currentCheckpoint.Length;
			cur = currentCheckpoint[index];
			pos = currentCheckpoint[index].transform.position;
			
			if( urgency && useSpawns) {
				urgencyCur = currentUrgencySpawn[index];
				urgencyPos = currentUrgencySpawn[index].transform.position;
			}
		}
	}

	public void ChangePoints(GameObject newPoint) {
		
		for (int i = 0; i < currentCheckpoint.Length; i++) {
			if (currentCheckpoint[i].transform.position == newPoint.transform.position) {
				if (pos != currentCheckpoint[i].transform.position) {
					checkpointActivated = true;
				}
				cur = currentCheckpoint[i];
				pos = currentCheckpoint[i].transform.position;
				index = i;
				if (urgency && useSpawns) {
					urgencyCur = currentUrgencySpawn[index];
					urgencyPos = currentUrgencySpawn[index].transform.position;
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
