﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperScript : EnemyClass 
{
	private float xSpeed = 10f;
	private float jump = 10f;

	// after cycles frames the direction switches
	public uint cycles = 100;
	public uint cycleOffset = 50;
	
	// number of frames to wait after completing a cycle to switching direction
	private uint delayCounter = 0;
	private uint turnAroundDelay = 100;

	// Use this for initialization
	void Start () {
		gravity = -40f;
		base.Start();
	}
	
	// Update is called once per frame
	void Update () {
		if (delayCounter != 0) 
		{
			delayCounter--;
			velocity.x = 0;
			ApplyGravity(ref velocity, Time.deltaTime);
			base.Update();
			return;
		}

		cycleOffset++;
		if (cycleOffset % cycles == 0)
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
		}
		
		if (c2d.collision.above) {
			velocity.y = 0;
		}

		HorizontalCollisionStop(ref velocity);

		c2d.Move(velocity*Time.deltaTime);
	}
}
