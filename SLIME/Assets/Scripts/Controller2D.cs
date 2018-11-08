using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour 
{
	
	public LayerMask collisionMask;
	public bool DebugFlag = true;

	public ControllerCollision collision; 
	public ControllerBounds bounds;
	
	private BoxCollider2D cc;
	private PlayerScript ps;
	private EnemyClass ec;
	// Use this for initialization
	void Start () 
	{
		cc = GetComponent<BoxCollider2D>();
		ps = GetComponent<PlayerScript>();
		ec = GetComponent<EnemyClass>();
		collision.reset();
		bounds.initRay(cc);
	}
	
//
//	Public Methods
//
/////////////////////////////////////

	/**
		Moves the player transform based on a given velocity
		taking into account collisions. Recalculates new
		collision information
		@param velocity: given velocity of player movement
	 */
	public void Move(Vector3 velocity) 
	{
		collision.reset();
		bounds.reload(cc);
		VerticalCollisions(ref velocity); 
		HorizontalCollisions(ref velocity);

		transform.Translate(velocity);
		bounds.reload(cc);
		if (DebugFlag) {DebugCollider();}
	}
	/**
		Similar to RigidBody.Raycast, this will take in
		a float v in a particular direction ((-) = down, (+) = up)
		it will check the distance abs(v) from that side of the 
		controller for collisions and returns the successful 
		raycast or an empty if it didn't hit anything
	 */
	public RaycastHit2D VerticalRaycast(float v)
	{
		float direction = Mathf.Sign(v);
		float speed = Mathf.Abs(v);
		// Doesn't use rays on the corner to allow for corner fudging
		for (int i = 1; i < bounds.hRayCount-1; i++)
		{
			Vector2 origin = bounds.bottomLeft;
			if (direction == 1) 
			{
				origin = bounds.topLeft;
			}
			Vector2 shift = new Vector2(i*bounds.hRaySpace, 0);
			
			RaycastHit2D hit = Physics2D.Raycast(origin+shift, direction*Vector2.up*v, speed, collisionMask);
			if (DebugFlag) { Debug.DrawRay(origin+shift, direction*Vector2.up*v, Color.yellow); }

			if (hit.collider != null) { return hit; }
		}
		return Physics2D.Raycast(Vector2.zero, Vector2.zero, 0, 0);
	}

	/**
		Same as previous, it takes in a float v in 
		a particular direction ((-) = left, (+) = right)
		and checks the distance abs(v) from that side of the 
		controller for collisions, returns the same as above
	 */
	public RaycastHit2D HorizontalRaycast(float v)
	{
		float direction = Mathf.Sign(v);
		float speed = Mathf.Abs(v);
		// Doesn't use rays on the corner to allow for corner fudging
		for (int i = 1; i < bounds.vRayCount-1; i++)
		{
			Vector2 origin = bounds.bottomLeft;
			if (direction == 1) 
			{
				origin = bounds.bottomRight;
			}
			Vector2 shift = new Vector2(0, i*bounds.vRaySpace);
			
			RaycastHit2D hit = Physics2D.Raycast(origin+shift, direction*Vector2.right*v, speed, collisionMask);
			if (DebugFlag) { Debug.DrawRay(origin+shift, direction*Vector2.right*v, Color.yellow); }

			if (hit.collider != null) { return hit; }
		}
		return Physics2D.Raycast(Vector2.zero, Vector2.zero, 0, 0);
	}
//
//	Private Methods
//
/////////////////////////////////////
	/** 
		Draws a box around the bounding box collider 
		as well as the outward raycasts
	 */
	private void DebugCollider() 
	{
		Debug.DrawLine(bounds.bottomLeft, bounds.bottomRight, Color.white);
		Debug.DrawLine(bounds.bottomLeft, bounds.topLeft,     Color.white);
		Debug.DrawLine(bounds.topRight,   bounds.bottomRight, Color.white);
		Debug.DrawLine(bounds.topRight,   bounds.topLeft,     Color.white);

		for (int i = 0; i < bounds.hRayCount; i++)
		{
			Vector2 shift = new Vector2(i*bounds.hRaySpace, 0);
			Debug.DrawRay(bounds.bottomLeft+shift, -Vector2.up*0.25f, collision.below?Color.blue:Color.red);
			Debug.DrawRay(bounds.topLeft+shift,     Vector2.up*0.25f, collision.above?Color.blue:Color.red);
		}	

		for (int i = 0; i < bounds.vRayCount; i++)
		{
			Vector2 shift = new Vector2(0, i*bounds.vRaySpace);
			Debug.DrawRay(bounds.bottomLeft+shift, -Vector2.right*0.25f, collision.left?Color.blue:Color.red);
			Debug.DrawRay(bounds.bottomRight+shift, Vector2.right*0.25f, collision.right?Color.blue:Color.red);
		}	
	}
	/** 
		Calculates the new velocity with respect to y
		@param velocity: the velocity to be changed
	 */
	private void VerticalCollisions(ref Vector3 velocity)
	{
		if (velocity.y == 0) { return; }
		
		float direction = Mathf.Sign(velocity.y);
		float speed = Mathf.Abs(velocity.y);

		// Doesn't use rays on the corner to allow for corner fudging
		for (int i = 1; i < bounds.hRayCount-1; i++)
		{
			Vector2 origin = bounds.bottomLeft;
			if (direction == 1) 
			{
				origin = bounds.topLeft;
			}
			Vector2 shift = new Vector2(i*bounds.hRaySpace, 0);
			
			RaycastHit2D hit = Physics2D.Raycast(origin+shift, direction*Vector2.up, speed, collisionMask);
			if (DebugFlag) { Debug.DrawRay(origin+shift, direction*Vector2.up, Color.green); }

			if (hit.collider != null) 
			{
				if (ps != null) {
					switch(hit.transform.tag)
					{
						case "hazard":
							ps.KillPlayer();
							break;
						case "tools":
							ToolsInterface t = hit.collider.gameObject.GetComponent<ToolsInterface>();
							t.Interact(gameObject);
							goto default;
						default:
							speed = VerticalCollide(ref velocity, hit, direction);
							break;
					}
				} else if (ec != null) {
					if (hit.transform.tag != "Player")
					{
						speed = VerticalCollide(ref velocity, hit, direction);
					}
				} else {

					speed = VerticalCollide(ref velocity, hit, direction);
				}
			}
		}
	}
	private float VerticalCollide(ref Vector3 velocity, RaycastHit2D hit, float direction)
	{
		velocity.y = hit.distance * direction;

		collision.above = direction ==  1;
		collision.below = direction == -1;

		return hit.distance;
	}

	/** 
		Calculates the new velocity with respect to x
		@param velocity: the velocity to be changed
	 */
	private void HorizontalCollisions(ref Vector3 velocity)
	{
		if (velocity.x == 0) { return; }

		float direction = Mathf.Sign(velocity.x);
		float speed = Mathf.Abs(velocity.x);

		// Doesn't use rays on the corner to allow for corner fudging
		for (int i = 1; i < bounds.vRayCount-1; i++)
		{
			Vector2 origin = bounds.bottomLeft;
			if (direction == 1) 
			{
				origin = bounds.bottomRight;
			}
			Vector2 shift = new Vector2(0, i*bounds.vRaySpace);
			
			RaycastHit2D hit = Physics2D.Raycast(origin+shift, direction*Vector2.right, speed, collisionMask);
			if (DebugFlag) { Debug.DrawRay(origin+shift, direction*Vector2.right, Color.green); }

			if (hit.collider != null) 
			{
				if (ps != null) {
					switch(hit.transform.tag)
					{
						case "hazard":
							ps.KillPlayer();
							break;
						case "tools":
							ToolsInterface t = hit.collider.gameObject.GetComponent<ToolsInterface>();
							t.Interact(gameObject);
							goto default;
						default:
							speed = HorizontalCollide(ref velocity, hit, direction);
							break;
					}				
				} else if (ec != null) {
					if (hit.transform.tag != "Player")
					{
						speed = HorizontalCollide(ref velocity, hit, direction);
					}
				}
				else {
					speed = HorizontalCollide(ref velocity, hit, direction);
				}
			}
		}
	}
	private float HorizontalCollide(ref Vector3 velocity, RaycastHit2D hit, float direction)
	{
		velocity.x = hit.distance * direction;

		collision.right = direction ==  1;
		collision.left  = direction == -1;
		return hit.distance;
	}
	/** 
		Struct to contain information on the bounding box
		as well as the ray cast information
		@method initRay:
			Initializes the values of the struct based on the bounding
			box for the rayCast values, calls reload.
			@param cc: collider of the controller
		@method reload:
			Sets the new bounding box coordinates
			@param cc: collider of the controller
	 */
	public struct ControllerBounds 
	{
		public Vector2 topLeft, topRight, bottomLeft, bottomRight;

		public int hRayCount, vRayCount;
		public float hRaySpace, vRaySpace;
		
		public void initRay(BoxCollider2D cc) 
		{
			Bounds b = cc.bounds;
			
			hRayCount = 10;
			vRayCount = 10;

			hRaySpace = b.size.x / (hRayCount-1);
			vRaySpace = b.size.y / (vRayCount-1);
			reload(cc);
		}
		public void reload(BoxCollider2D cc) 
		{
			Bounds b = cc.bounds;

			topLeft     = new Vector2(b.min.x, b.max.y);
			topRight    = new Vector2(b.max.x, b.max.y);
			bottomLeft  = new Vector2(b.min.x, b.min.y);
			bottomRight = new Vector2(b.max.x, b.min.y);
		}
	}
	
	/** 
		Struct to contain information what sides are colliding
		with the next translation
		@method resets:
			sets all fields to false to be recalculate
	 */
	public struct ControllerCollision 
	{
		public bool above, below, right, left;

		public void reset() 
		{
			above = false;
			below = false;
			right = false;
			left  = false;
		}	
	}
}
