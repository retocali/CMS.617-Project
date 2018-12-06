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

	public int[] spawnUrgencyAt;

	public int[] destroyUrgencyAt;

	private int urgencyIndex = 0;

	bool urgencyAlive = false;

	public int urgencySpeed = 2;


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

	// Returns the index of the current checkpoint (zero index)
	public int getCurrentCheckpointIndex(){

		return index;
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
			if (currentUrgencySpawn.Length > 0 && checkList(index, spawnUrgencyAt)) {
				urgencyAlive = true;
				urgencyCur = currentUrgencySpawn[0];
				urgencyPos = urgencyCur.transform.position;
				spawnUrgency();
				
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
				Debug.Log("DIE URG");
				Destroy(urgency);
				spawnUrgency();
			}
			var splitterScript = gameObject.GetComponent<SplitterMasterScript>();
			
			if (splitterScript) {
				splitterScript.resetSplitters();
			}
		}
		if (Input.GetKeyDown("1") && Application.isEditor)
		{
			index = (index + 1) % currentCheckpoint.Length;
			cur = currentCheckpoint[index];
			pos = currentCheckpoint[index].transform.position;
			
			if ( urgency != null && useSpawns) {				
				if (checkList(index, spawnUrgencyAt)) {
					urgencyCur = currentUrgencySpawn[urgencyIndex];
					urgencyPos = currentUrgencySpawn[urgencyIndex].transform.position;
					spawnUrgency();
					urgencyAlive = true;
				}
				else if (checkList(index, destroyUrgencyAt)) {
					urgencyAlive = false;
					urgencyIndex = (urgencyIndex + 1) % currentUrgencySpawn.Length;
					Debug.Log("URGENCY GONE");
					killUrgency(urgency);

				}
				else if( urgencyAlive) {
					urgencyIndex = (urgencyIndex + 1) % currentUrgencySpawn.Length;
				}
				urgencyCur = currentUrgencySpawn[urgencyIndex];
				urgencyPos = currentUrgencySpawn[urgencyIndex].transform.position;
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

					if (checkList(i, destroyUrgencyAt)) {
						urgencyIndex++;
						urgencyAlive = false;
						Debug.Log("URGENCY GONE");
						killUrgency(urgency);
					}
					if (useSpawns &&urgencyAlive) {
						urgencyIndex++;
						Debug.Log("URGENCY INDEX");
						Debug.Log(urgencyIndex);
					}
					if (checkList(i, spawnUrgencyAt)) {
						Debug.Log("URGENCY INDEX");
						Debug.Log(urgencyIndex);
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
					spawnUrgency();
					
				}
			} 
			else {
				currentCheckpoint[i].GetComponent<SpriteRenderer>().color =
					currentCheckpoint[i].GetComponent<spawnPlayer>().defaultColor;	
			}
		}
	}

	public void SpawnPlayer() {
		gameObject.GetComponent<SplitterMasterScript>().resetSplitters();
		player.GetComponent<PlayerScript>().SpawnPlayer(pos);
		ReloadMaster.ReloadObjects();
	}

	private bool checkList(int i, int[] list) {
		for (int x = 0; x < list.Length; x ++) {
			if (list[x] == i) {
				return true;
			}
		}
		return false;
	}

	private void spawnUrgency(){
		MusicMaster.SpawnUrgency();
		urgency = urgencyCur.GetComponent<EnemySpawnScript>().Spawn();
		urgency.GetComponent<urgency>().changeSpeed(urgencySpeed);
	}

	private void killUrgency(GameObject u) {
		MusicMaster.DespawnUrgency();
		Destroy(u);

	}

}
