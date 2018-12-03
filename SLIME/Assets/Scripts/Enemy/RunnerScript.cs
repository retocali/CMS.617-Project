
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class RunnerScript : EnemyClass {

	public AudioClip switchSound;
	private float xAcceleration = 20f;

	// after cycles frames the direction switches
	public uint cycles = 100;
	public uint cycleOffset = 50;
	private uint currentCycle;
	
	// number of frames to wait after completing a cycle to switching direction
	private uint delayCounter = 0;
	private uint turnAroundDelay = 100;

	// Use this for initialization
	new void Start () {
		gravity = -20f;
		currentCycle = cycleOffset;
		base.Start();
		GetComponent<SpriteRenderer>().flipX = xAcceleration > 0;
	}
	
	public override void Respawn()
	{
		delayCounter = 0;
		currentCycle = cycleOffset;
		GetComponent<SpriteRenderer>().flipX = xAcceleration > 0;
		base.Respawn();
	}

	// Update is called once per frame
	new void Update () {
		if (delayCounter != 0) 
		{
			if (audsrc.isPlaying()) {audsrc.Stop();}
			delayCounter--;
			velocity.x = 0;
			animor.SetBool("idle", true);
			ApplyGravity(ref velocity, Time.deltaTime);
			c2d.Move(velocity*Time.deltaTime);
			return;
		}
		
		currentCycle++;
		if (currentCycle % cycles == 0)
		{
			xAcceleration = -xAcceleration;
			audsrc.PlayOneShot(switchSound);
			if (turnAroundDelay > 0) 
			{
				animor.SetBool("idle", true);
				GetComponent<SpriteRenderer>().flipX = xAcceleration > 0;
				velocity.x = 0;
			}	
			delayCounter = turnAroundDelay;
			audsrc.Play();
			return;
		}
		
		animor.SetBool("idle", false);
		ApplyGravity(ref velocity, Time.deltaTime);
		velocity.x += Time.deltaTime*xAcceleration;
		animor.SetFloat("speed", velocity.x);
		base.Update();
	}
}
