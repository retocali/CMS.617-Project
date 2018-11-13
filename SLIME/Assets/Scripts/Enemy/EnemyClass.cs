using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class EnemyClass : MonoBehaviour {

	protected Controller2D c2d;
    protected Animator animor;
	
	public    bool spawned     = false;
	protected bool dead       = false;
	protected bool functional = true;

	protected Vector3 velocity      = Vector3.zero;
	protected Vector3 initialLoc    = Vector3.zero;
	protected Vector3 addedVelocity = Vector3.zero;

	protected float gravity          = 0f;
	protected float velocityModifier = 1f;

	protected void Start() 
	{
		c2d = GetComponent<Controller2D>();
		initialLoc = transform.position;
		animor = GetComponentInChildren<Animator>();
		ReloadMaster.AddToMaster(this);
	}
	protected void Update() 
	{
		ApplyVelocityModifiers();
		c2d.Move(velocity*Time.deltaTime);
		
		if (animor != null) {
			animor.SetFloat("vspeed", velocity.y);
		}

		VerticalCollisionStop(ref velocity);
		HorizontalCollisionStop(ref velocity);
	}

	public void Respawn()
	{
		dead = false;
		transform.position = initialLoc;
   		velocity = Vector3.zero;
	}

	public void KillEnemy()
	{
		dead = true;
	}

	protected void Warn(string s)
	{
		Debug.LogError(s);
		functional = false;
	}	
	/**
		These methods are used for clamping velocities when colliding
	 */
	protected void HorizontalCollisionStop(ref Vector3 velocity)
	{
		if (c2d.collision.right || c2d.collision.left) 
		{
			velocity.x = 0;
		}
	}
	protected void VerticalCollisionStop(ref Vector3 velocity)
	{
		if (c2d.collision.above || c2d.collision.below) 
		{
			velocity.y = 0;
		}
	}

	/**
		Applies the acceleration of gravity to the velocity
		over the time step given and returns true if it is outta bounds
	 */
	protected bool ApplyGravity(ref Vector3 velocity, float t)
	{
		velocity.y += t*gravity;
		return false;
	}

	/**
		A method used for adding velocity to the enemy
		negative numbers will decrease speed. Accumulates
		Note: this uses a vector2 which is different from the
		player's method
	 */
	public void AddVelocity(Vector3 v)
	{
		addedVelocity += v;
	}
	/**
		Used to multiply the velocity of the player
		in a safe manner, overwrites previous value;
	 */
	public void MultiplyVelocity(float f)
	{
		velocityModifier = f;
	}

	/**
		Actually applies the above two methods to the velocity
	 */
	protected void ApplyVelocityModifiers()
	{
		velocity += addedVelocity;
		addedVelocity = Vector3.zero;

		velocity *= velocityModifier;
		velocityModifier = 1;
	}	
	
}
