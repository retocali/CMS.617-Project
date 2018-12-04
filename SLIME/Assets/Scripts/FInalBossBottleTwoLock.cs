using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FInalBossBottleTwoLock : MonoBehaviour {

    private TilemapCollider2D tc2d;
    private TilemapRenderer tmr;

	private FinalBossScript bossScript;

	private bool hasShownGates = false;

	private CameraScript camera;

	// Use this for initialization
	void Start () {
		

        tc2d = GetComponent<TilemapCollider2D>();
        tmr = GetComponent<TilemapRenderer>();

		bossScript = GameObject.FindGameObjectWithTag("FinalBoss").GetComponent<FinalBossScript>();

		camera = Camera.main.gameObject.GetComponent<CameraScript>();

	}
	
	// Update is called once per frame
	void Update () {
		if(bossScript.health < 2){
			if(hasShownGates == false){
				StartCoroutine("disableLocks");
			}
			hasShownGates = true;

		}else{
			hasShownGates = false;
			tc2d.enabled = true;
			tmr.enabled = true;
		}
	}

	IEnumerator disableLocks() {
		FinalBossScript.shouldShoot = false;
		DestroyAllObjectsWithTag("fireball");
		camera.CameraFocusTimed(new Vector2(66, 11), 2.0f);
		yield return new WaitForSeconds(2);
		tc2d.enabled = false;
		tmr.enabled = false;
		camera.CameraFocusTimed(new Vector2(66, 11), 1.0f);
		yield return new WaitForSeconds(1);
		FinalBossScript.shouldShoot = true;
	}

	void DestroyAllObjectsWithTag(string tag)
	{
		var gameObjects = GameObject.FindGameObjectsWithTag (tag);
		
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}
	}
}
