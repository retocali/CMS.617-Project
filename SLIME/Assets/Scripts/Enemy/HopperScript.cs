using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperScript : EnemyClass 
{
	private float xSpeed = 10f;
	private float jump = 10f;
	public AudioClip hopSound;

	// after cycles frames the direction switches
	public uint cycles = 100;
	public uint cycleOffset = 50;
	private uint currentCycle = 0;
	
	// number of frames to wait after completing a cycle to switching direction
	private uint delayCounter = 0;
	private uint turnAroundDelay = 100;
	
    private SpriteRenderer sprend;

	// Use this for initialization
	new void Start () {
		gravity = -30f;
		currentCycle = cycleOffset;
        animor = GetComponent<Animator>();
        sprend = GetComponent<SpriteRenderer>();
		base.Start();
	}
	
	public override void Respawn()
	{
		delayCounter = 0;
		currentCycle = cycleOffset;
		base.Respawn();
	}

	// Update is called once per frame
	new void Update () {

        animor.SetFloat("vspeed", velocity.y);

        if(velocity.x < 0)
        {
            sprend.flipX = false;
        }
        else if(velocity.x > 0)
        {
            sprend.flipX = true;
        }

		if (delayCounter != 0) 
		{
			delayCounter--;
			velocity.x = 0;
			ApplyGravity(ref velocity, Time.deltaTime);
			base.Update();
			return;
		}

		currentCycle++;
		if (currentCycle % cycles == 0)
		{
			xSpeed = -xSpeed;
			
			if (turnAroundDelay > 0) 
			{
				velocity.x = 0;
			}	
			delayCounter = turnAroundDelay;
		}

		velocity.x = xSpeed;
		ApplyGravity(ref velocity, Time.deltaTime); 
		ApplyVelocityModifiers();
		if (c2d.collision.below) { 
			velocity.y = jump; 
			audsrc.PlayOneShot(hopSound, 0.5f);
		}
		
		if (c2d.collision.above) {
			velocity.y = 0;
		}

		HorizontalCollisionStop(ref velocity);

		c2d.Move(velocity*Time.deltaTime);
	}
}
