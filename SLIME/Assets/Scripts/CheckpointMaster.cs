using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointMaster : MonoBehaviour {

	public GameObject[] currentCheckpoint;

	private GameObject cur;

	private Vector3 pos;

	private GameObject player;

	private int index = 0;

	// Use this for initialization
	void Start () {
		cur = currentCheckpoint[0];
		pos = cur.transform.position;
		player = cur.GetComponent<spawnPlayer>().Spawn();


		
	}
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetButtonDown("1"))
        // {
        //     Debug.Log(Input.mousePosition);
        // }

		if (player == null) {
			SpawnPlayer();
		}

		if (Input.GetKeyDown("1"))
        {
			index = (index + 1) % currentCheckpoint.Length;
            cur = currentCheckpoint[index];
			pos = currentCheckpoint[index].transform.position;
			Debug.Log(index);
        }
	}

	public void ChangePoints(GameObject newPoint) {
		for(int i=0; i < currentCheckpoint.Length; i++){
			if(currentCheckpoint[i].transform.position == newPoint.transform.position) {
				cur = currentCheckpoint[i];
				pos = currentCheckpoint[i].transform.position;
				Debug.Log(cur);
				Debug.Log(pos);
				index = i;
			}
		}
	}

	public void SpawnPlayer() {
		player = cur.GetComponent<spawnPlayer>().Spawn();

	}


}
