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

	public int spawnUrgencyAt = 0;

	public int destroyUrgencyAt = 0;

	private int urgencyIndex = 0;

	bool urgencyAlive = false;

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
		return checkpointActivated;
	}

	////////////////////////////////////////////////
	// Private Methods
	///////////////////////////////////////////////

	// Use this for initialization
	void Start () {
		Debug.Assert(currentCheckpoint.Length >= currentUrgencySpawn.Length);
		if (currentCheckpoint.Length > 0) {
			cur = currentCheckpoint[0];
			pos = cur.transform.position;
		} else {
			Debug.LogWarning("Could not find any checkpoints");
		}

		if ( urgency == null && useSpawns) {
			if (currentUrgencySpawn.Length > 0 && index == spawnUrgencyAt) {
				urgencyAlive = true;
				urgencyCur = currentUrgencySpawn[0];
				urgencyPos = urgencyCur.transform.position;
				urgency = urgencyCur.GetComponent<EnemySpawnScript>().Spawn();
			} else {
				Debug.LogWarning("Could not find sense of urgency");
			}
		}
		if (urgency != null && !useSpawns) {
			lastUrgencyPos = urgency.transform.position;
		}
		if (urgency == null && !useSpawns) {
			Debug.LogWarning("No Urgency Being Used");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<PlayerScript>().IsDead() == true) {
			SpawnPlayer();
			if (urgency != null && useSpawns && urgencyAlive) {
				Destroy(urgency);
				urgency = urgencyCur.GetComponent<EnemySpawnScript>().Spawn();
			}
		}
		if (Input.GetKeyDown("1"))
		{
			index = (index + 1) % currentCheckpoint.Length;
			cur = currentCheckpoint[index];
			pos = currentCheckpoint[index].transform.position;

			if (index == urgencyIndex) {
				urgencyAlive = true;
			} 
			
			if ( urgency != null && useSpawns && urgencyAlive) {
				urgencyIndex = (index + 1) % currentUrgencySpawn.Length;
				if(urgencyIndex == 0 && index < spawnUrgencyAt){
					urgencyAlive = false;
					Debug.Log("URGENCY GONE");

				} 
				else if (urgencyIndex == destroyUrgencyAt) {
					urgencyAlive = false;
					Debug.Log("URGENCY GONE");

				}
				urgencyCur = currentUrgencySpawn[index - spawnUrgencyAt];
				urgencyPos = currentUrgencySpawn[index - spawnUrgencyAt].transform.position;
			}
		}
	}

	public void ChangePoints(GameObject newPoint) {
		
		for (int i = 0; i < currentCheckpoint.Length; i++) {
			if (currentCheckpoint[i].transform.position == newPoint.transform.position) {
				if (pos != currentCheckpoint[i].transform.position) {
					if ( urgency != null) {
						lastUrgencyPos = urgency.transform.position;
					}

					checkpointActivated = true;

					if (i == destroyUrgencyAt) {
						urgencyAlive = false;
						Debug.Log("URGENCY GONE");
						Destroy(urgency);
					}
					if (useSpawns &&urgencyAlive) {
						urgencyIndex++;
						Debug.Log("URGENCY INDEX");
						Debug.Log(urgencyIndex);
					}
					if (i == spawnUrgencyAt) {
						urgencyAlive = true;
					}

				}
				cur = currentCheckpoint[i];
				pos = currentCheckpoint[i].transform.position;
				index = i;

				if (urgency != null && useSpawns && urgencyAlive) {
					urgencyCur = currentUrgencySpawn[urgencyIndex];
					urgencyPos = currentUrgencySpawn[urgencyIndex].transform.position;


				}
				else if (urgency == null && useSpawns && urgencyAlive) {
					urgencyCur = currentUrgencySpawn[urgencyIndex];
					urgencyPos = currentUrgencySpawn[urgencyIndex].transform.position;
					urgency = urgencyCur.GetComponent<EnemySpawnScript>().Spawn();
					
				}
			} 
			else {
				currentCheckpoint[i].GetComponent<MeshRenderer>().material.color =
					currentCheckpoint[i].GetComponent<spawnPlayer>().defaultColor;	
			}
		}
	}

	public void SpawnPlayer() {
		gameObject.GetComponent<SplitterMasterScript>().resetSplitters();
		player.GetComponent<PlayerScript>().SpawnPlayer(pos);
		ReloadMaster.ReloadObjects();
	}

}
