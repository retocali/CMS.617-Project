using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FinalBossGateManager : MonoBehaviour {

    private CheckpointMaster checkpointMaster;

    private TilemapCollider2D tc2d;
    private TilemapRenderer tmr;

    private CameraScript camera;

    private Transform bossTransform;

    public static bool hasShownBoss = false;

	// Use this for initialization
	void Start () {
        checkpointMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CheckpointMaster>();

        tc2d = GetComponent<TilemapCollider2D>();
        tmr = GetComponent<TilemapRenderer>();

        tc2d.enabled = false;
        tmr.enabled = false;

        camera = Camera.main.gameObject.GetComponent<CameraScript>();
        bossTransform = GameObject.FindGameObjectWithTag("FinalBoss").transform;
    }
	
	// Update is called once per frame
	void Update () {
		if(checkpointMaster.getCurrentCheckpointIndex() > 0)
        {
            tc2d.enabled = true;
            tmr.enabled = true;
            if(hasShownBoss == false){
                StartCoroutine("showBoss");
            }
            hasShownBoss = true;
        }



	}

    IEnumerator showBoss(){
        camera.CameraFocusTimed(new Vector2(bossTransform.position.x, bossTransform.position.y), 2);
        yield return new WaitForSeconds(2);
        FinalBossScript.shouldShoot = true;
    }
}
