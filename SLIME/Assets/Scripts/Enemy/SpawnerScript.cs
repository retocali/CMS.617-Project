using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : EnemyClass
{
	public GameObject spawnPrefab;
	
	public bool sensitive = false;
	public float time = 2f;
	public int number = 10;
	
	private float currentTime = 0;
	private float speed = 15f;
	private Transform player;
	private MeshRenderer mesh;

	private float XRange = 10;
	private float YRange = 5;
	// Use this for initialization
	void Start () 
	{
		if (spawnPrefab == null)
		{
			Warn("Prefab not set");
		}
		mesh = GetComponent<MeshRenderer>();
	}
	// Update is called once per frame
	void Update () 
	{
		if (!functional) { Warn(transform.name+" not functional"); return; }
		if (sensitive && PlayerInRange())
		{
			Debug.Log(time-currentTime);
			currentTime += Time.deltaTime;
			mesh.material.color = new Color(1-currentTime/time, 0, 0);
		}
		
		if (currentTime >= time) 
		{
			currentTime = 0;
			mesh.material.color = Color.black;
			Spawn();
		}
	}

	private bool PlayerInRange()
	{
		if (!sensitive) { return true; }
		if (player == null)
		{
			player = PlayerScript.FindPlayer().transform;
		}

		return (Mathf.Abs(player.position.x-transform.position.x) < XRange
		     && Mathf.Abs(player.position.y-transform.position.y) < YRange);
	}
	private void Spawn()
	{
		float step = (Mathf.PI/number);
		for (float rad = Mathf.PI/4; rad < 3*Mathf.PI/4; rad += step)
		{
			Vector3 displacement = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
			var s = (GameObject) Instantiate(spawnPrefab,
											 this.transform.position,
											 this.transform.rotation);
			s.GetComponent<EnemyClass>().AddVelocity(speed*displacement);
		}
	
	}
}
