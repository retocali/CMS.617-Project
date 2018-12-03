using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FinalBossGateManager : MonoBehaviour {

    private CheckpointMaster checkpointMaster;

    private TilemapCollider2D tc2d;
    private TilemapRenderer tmr;

	// Use this for initialization
	void Start () {
        checkpointMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CheckpointMaster>();

        tc2d = GetComponent<TilemapCollider2D>();
        tmr = GetComponent<TilemapRenderer>();

        tc2d.enabled = false;
        tmr.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(checkpointMaster.getCurrentCheckpointIndex() > 0)
        {
            tc2d.enabled = true;
            tmr.enabled = true;
        }
	}
}
