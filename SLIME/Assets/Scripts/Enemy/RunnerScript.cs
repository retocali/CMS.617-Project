using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class RunnerScript : EnemyClass {

	private float xAcceleration = 20f;

	// after cycles frames the direction switches
	public uint cycles = 100;
	public uint cycleOffset = 50;
	
	// number of frames to wait after completing a cycle to switching direction
	private uint delayCounter = 0;
	private uint turnAroundDelay = 100;

	// Use this for initialization
	new void Start () {
		gravity = -20f;
		base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
		if (delayCounter != 0) 
		{
			delayCounter--;
			velocity.x = 0;
			ApplyGravity(ref velocity, Time.deltaTime);
			c2d.Move(velocity*Time.deltaTime);
			return;
		}

		cycleOffset++;
		if (cycleOffset % cycles == 0)
		{
			xAcceleration = -xAcceleration;
			
			if (turnAroundDelay > 0) 
			{
				velocity.x = 0;
			}	
			delayCounter = turnAroundDelay;
			return;
		}
		ApplyGravity(ref velocity, Time.deltaTime);
		velocity.x += Time.deltaTime*xAcceleration;
		base.Update();
	}
}
