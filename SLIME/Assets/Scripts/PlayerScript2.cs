using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerScript2 : MonoBehaviour {

	public float jumpTime = 0.5f;
	public float jumpHeight = 10f;
	public float acceleration = 10f;

	private float minWallJumpSpeed = 1f;
	private float wallJumpAngle = Mathf.Deg2Rad*37.5f;
	private float wallJumpModifier = 1.5f;
	private float extendJumpModifier = 1.5f;
	private float increaseGravityModifier = 1.5f;
	private float maxSpeedY = 25f;
	private float maxSpeedX = 15f;

	private float gravity;
	private float jumpVelocity;
	private Vector3 velocity;
	private Vector3 prevVelocity;
	private Controller2D c2d;
	private MeshRenderer mesh;

	private bool stunned = false;
	private float stunCounter = 0;
	// Use this for initialization
	void Start () {
		float t = jumpTime/2;
		jumpVelocity = 2.0f*jumpHeight/t;
		gravity = -jumpVelocity/t;
		velocity = new Vector3(0,0,0);
		Debug.Log("Jump: " + jumpVelocity);
		Debug.Log("Gravity: " + gravity);
		mesh = GetComponent<MeshRenderer>();
		c2d = GetComponent<Controller2D>();
	}
	
	void Stun(float count) {
		mesh.material.color = Color.red;
		stunned = true;
		stunCounter = count;
	}
	void UnStun() {
		stunCounter -= Time.deltaTime;
		if (stunCounter <= 0) { 
			stunCounter = 0; 
			stunned = false;
			mesh.material.color = Color.green;
		}
	}

	// Update is called once per frame
	void Update () {
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 
									Input.GetAxisRaw("Vertical"),
									Input.GetAxisRaw("Jump"));
		Vector3 prevVelocity = velocity;

		if (c2d.collision.below || c2d.collision.above) {
			velocity.y = 0;
			if (prevVelocity.y < -2*jumpVelocity) {
				Stun((-prevVelocity.y-2*jumpVelocity)/10f);
			}
		}
		if (c2d.collision.right || c2d.collision.left) {
			if (input.z != 0 && Mathf.Abs(velocity.x) > minWallJumpSpeed) {
				// velocity.y = Mathf.Min(jumpVelocity*Mathf.Abs(velocity.x/maxSpeedX), jumpVelocity);
				velocity.y = wallJumpModifier*jumpVelocity*Mathf.Sin(wallJumpAngle);
				velocity.x = wallJumpModifier*jumpVelocity*Mathf.Cos(wallJumpAngle)*-Mathf.Sign(velocity.x);
				Debug.Log(velocity);
			// 	Debug.Log("WallJump: " + velocity.x/maxSpeedX);
			} else {
				// velocity.x = 0;
				velocity.x *= -0.5f;
			}
		} 
		
		if (stunned) {
			UnStun();
			return;
		}
		velocity.x += input.x * acceleration * Time.deltaTime;
		if (input.x == 0) {
			velocity.x = 0;
		}

		if (c2d.collision.below || c2d.collision.above) {
			velocity.y = c2d.collision.below? jumpVelocity:-jumpVelocity;
			if (input.y == 1) {
				velocity.y *= extendJumpModifier;
			}
			if (input.y == -1) {
				velocity.y *= increaseGravityModifier;
			}
			Debug.Log("Jump: " + velocity.y);
		}
		
		float gravityDelta = gravity * Time.deltaTime;
		if (input.y == -1) {
			gravityDelta *= increaseGravityModifier*increaseGravityModifier;
		}
		Debug.Log("Gravity: " + gravityDelta/Time.deltaTime);
		velocity.y += gravityDelta;	

		// velocity.y = Mathf.Max(Mathf.Min(maxSpeedY, velocity.y), -maxSpeedY);
		// velocity.x = Mathf.Max(Mathf.Min(maxSpeedX, velocity.x), -maxSpeedX);

		c2d.Move(velocity*Time.deltaTime);
	}
}
