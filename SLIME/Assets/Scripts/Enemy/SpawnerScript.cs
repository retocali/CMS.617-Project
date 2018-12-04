using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : EnemyClass
{
	public GameObject spawnPrefab;
	
	public AudioClip releaseSound;
	private AudioSource audsrc;
	
	public bool sensitive = false;
	public float time = 2f;
	public int number = 10;
	
	private float currentTime = 0;
	private float speed = 15f;
	private Transform player;
	private SpriteRenderer sprend;

	public float XRange = 10;
	public float YRange = 5;
	// Use this for initialization
	new void Start () 
	{
		if (spawnPrefab == null)
		{
			Warn("Prefab not set");
		}
		sprend = GetComponent<SpriteRenderer>();
		initialLoc = transform.position;
		ReloadMaster.AddToMaster(this);
		audsrc.GetComponent<AudioSource>();
	}
	
	public override void Respawn()
	{
		currentTime = 0;
		sprend.color = Color.white;
		base.Respawn();
	}

	// Update is called once per frame
	new void Update () 
	{
		if (!functional) { 
			Warn(transform.name+" not functional"); 
			return; 
		}
		if (sensitive && PlayerInRange())
		{
			currentTime += Time.deltaTime;
			if (currentTime/time < 1) {
				sprend.color = new Color(1-currentTime/time, 0, 0);
			} 
			else {
				sprend.color = Color.black;
			}


		}
		
		if (currentTime >= time) 
		{
			currentTime = 0;
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
		audsrc.PlayOneShot(releaseSound);
		float step = (Mathf.PI/number);
		for (float rad = Mathf.PI/4; rad < 3*Mathf.PI/4; rad += step)
		{
			Vector3 displacement = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
			var s = (GameObject) Instantiate(spawnPrefab,
											 this.transform.position,
											 this.transform.rotation);
			s.GetComponent<EnemyClass>().spawned = true;
			s.GetComponent<EnemyClass>().AddVelocity(speed*displacement);
		}
	
	}
}
