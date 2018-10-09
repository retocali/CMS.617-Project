using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerScript2 : MonoBehaviour {

	public float jumpTime = 0.5f;
	public float jumpHeight = 10f;
	public float acceleration = 10f;

	private float minWallJumpSpeed = 1f;
	private float wallJumpAngle = Mathf.Deg2Rad*35f;
	private float wallJumpModifier = 1.1f;

	private float jumpVelocity;
	private float extendJumpModifier = 1.5f;
	
	private float gravity;
	private float increaseGravityModifier = 1.5f;
	
	private float maxSpeedY = 100f;
	private float maxSpeedX = 15f;

	private Vector3 prevVelocity;
	private Vector3 velocity = new Vector3(0,0,0);
	
	private Controller2D c2d;
	private MeshRenderer mesh;

	private bool dead = false;
	private bool stunned = false;
	private float stunCounter = 0;
	private float stunDropModifier = 2.5f;

	// Use this for initialization
	void Start () 
	{
	
		float t = jumpTime/2;
		jumpVelocity = 2.0f*jumpHeight/t;
		gravity = -jumpVelocity/t;
		
	
		mesh = GetComponent<MeshRenderer>();
		c2d = GetComponent<Controller2D>();
	}

//
//	 Public Methods
//
///////////////////////////////////////

	/**
		"Kills" the player by setting an internal flag
		to true; 
	 */
	public void KillPlayer()
	{
		mesh.material.color = Color.red;
		dead = true;
		velocity = new Vector3(0,0,0);
	}
	
	/**
		Returns whether the player is alive or not
	 */
	public bool IsAlive() 
	{
		return dead; 
	}

	/**
		"Stuns" the player which causes them to be unable to 
		move in any direction 
		@param count: seconds for how long the player will be
		stunned
	 */
	public void Stun(float count) 
	{
		Debug.Log(velocity);
		mesh.material.color = Color.red;
		velocity = new Vector3(0,0,0);
		prevVelocity = new Vector3(0,0,0);
		stunned = true;
		stunCounter = count;
	}

//
//	 Private Methods
//
///////////////////////////////////////

	/**
		Complement to above, is used to eventually
		"unstun" the character or allow them to move again.
	 */
	private void UnStun() 
	{
		stunCounter -= Time.deltaTime;
		velocity = new Vector3(0,0,0);
		prevVelocity = new Vector3(0,0,0);
		if (stunCounter <= 0)
		{ 
			stunCounter = 0; 
			stunned = false;
			mesh.material.color = Color.green;
			return;
		}
	}

	/**
		Controls the calculation of velocities based on the 
		bounces the player must make. This calculates wall jumps,
		ground pounds, high jumps, and high-velocity splats
		
		@param velocity: current player velocity to be adjusted
		@param input: vector3 contained player inputs
	 */
	private void Bounce(ref Vector3 velocity, Vector3 input)
	{
		if (c2d.collision.below || c2d.collision.above) 
		{
			velocity.y = c2d.collision.below? jumpVelocity:-jumpVelocity;
			float maxDrop = -jumpVelocity*stunDropModifier;
			if (prevVelocity.y < maxDrop) 
			{
				Stun(Mathf.Abs(prevVelocity.y-maxDrop)*0.1f);
			}
			else if (input.y ==  1) { velocity.y *= extendJumpModifier; }
			else if (input.y == -1) { velocity.y *= increaseGravityModifier; }
		}
		if (c2d.collision.right || c2d.collision.left) 
		{
			if (input.z != 0) 
			{
				float speed = Mathf.Sqrt(velocity.x*velocity.x+velocity.y*velocity.y);
				if (speed > minWallJumpSpeed) 
				{
					velocity.y = wallJumpModifier*speed*Mathf.Sin(wallJumpAngle);
					velocity.x = wallJumpModifier*speed*Mathf.Cos(wallJumpAngle)*-Mathf.Sign(velocity.x);
				}
			} 
			else 
			{
				velocity.x *= -0.5f;
			}
		} 
	}

	/**
		Controls the calculation of velocities in x direction
		and clamping it.
		
		@param velocity: current player velocity to be adjusted
		@param input: vector3 contained player inputs
	 */
	private void Move(ref Vector3 velocity, Vector3 input)
	{
		velocity.x += input.x * acceleration * Time.deltaTime;
		if (input.x == 0) { velocity.x = 0; }
		velocity.x = Mathf.Max(Mathf.Min(maxSpeedX, velocity.x), -maxSpeedX);
	} 

	/**
		Applies gravity in the y direction and clamps y velocity

		@param velocity: current player velocity to be adjusted
		@param input: vector3 contained player inputs
	 */
	private void ApplyGravity(ref Vector3 velocity, Vector3 input)
	{
		float gravityDelta = gravity * Time.deltaTime;
		if (input.y == -1) { gravityDelta *= increaseGravityModifier*increaseGravityModifier; }
		velocity.y += gravityDelta;	

		velocity.y = Mathf.Max(Mathf.Min(maxSpeedY, velocity.y), -maxSpeedY);
	}

	// Update is called once per frame
	void Update () {
		if (dead) { return; }
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 
									Input.GetAxisRaw("Vertical"),
									Input.GetAxisRaw("Jump"));

		
		
		if (stunned) { UnStun(); }
		else {
			Bounce(ref velocity, input);
			Move  (ref velocity, input);
		}
		
		ApplyGravity(ref velocity, input);

		prevVelocity = velocity;
		c2d.Move(prevVelocity*Time.deltaTime);
	}
}
