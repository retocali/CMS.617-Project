using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlierScript : EnemyClass {

	public AudioClip switchSound;
	public Vector2[] directions;
	public float[] times;

	private int i;
	private float currentTime;
	private float speed = 10f;

	// Use this for initialization
	new void Start () {
		i = 0;
		currentTime = 0;
		if (directions.Length == 0 || times.Length == 0) {
			Warn("Directions and times must not be empty");
		}
		if (directions.Length != times.Length) {
			Warn("Directions and times must be the same length!");
		}
		base.Start();
	}
	
	public override void Respawn()
	{
		i = 0;
		currentTime = 0;
		base.Respawn();
	}

	// Update is called once per frame
	new void Update () {
		if (!functional) { Warn(transform.name+" not functional"); return; }

		currentTime += Time.deltaTime;
		if (currentTime >= times[i]) {
			currentTime = 0;
			i = (i+1) % directions.Length;
			audsrc.PlayOneShot(switchSound, 0.15f);
		}
		
		velocity.x = speed * directions[i].x;
		velocity.y = speed * directions[i].y;
		
		base.Update();
	}
}
