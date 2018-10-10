using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class RunnerScript : MonoBehaviour {

	private Vector3 velocity;
	private Controller2D c2d;
	private float xAcceleration = 20f;
	private float gravity = -20f;

	// after cycles frames the direction switches
	public uint cycles = 100;
	public uint cycleOffset = 50;
	
	// number of frames to wait after completing a cycle to switching direction
	private uint delayCounter = 0;
	private uint turnAroundDelay = 100;

	// Use this for initialization
	void Start () {
		velocity = Vector3.zero;
		c2d = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (delayCounter != 0) 
		{
			delayCounter--;
			velocity.x = 0;
			velocity.y += Time.deltaTime*gravity;
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
		
		velocity.x += Time.deltaTime*xAcceleration;
		velocity.y += Time.deltaTime*gravity;
		if (c2d.collision.above || c2d.collision.below) {
			velocity.y = 0;
		}
		if (c2d.collision.right || c2d.collision.left) {
			velocity.x = 0;
		}

		c2d.Move(velocity*Time.deltaTime);
	}
}
