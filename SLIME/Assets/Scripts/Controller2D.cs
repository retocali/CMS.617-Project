using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour 
{
	public LayerMask collisionMask;
	public ControllerCollision collision; 
	public ControllerBounds bounds;
	public bool DebugFlag = true;
	private BoxCollider2D cc;
	
	// Use this for initialization
	void Start () 
	{
		cc = GetComponent<BoxCollider2D>();
		collision.reset();
		bounds.initRay(cc);
	}
	
	/** 
		Draws a box around the bounding box collider 
		as well as the outward raycasts
	 */
	void DebugCollider() 
	{
		Debug.DrawLine(bounds.bottomLeft, bounds.bottomRight, Color.white);
		Debug.DrawLine(bounds.bottomLeft, bounds.topLeft,     Color.white);
		Debug.DrawLine(bounds.topRight,   bounds.bottomRight, Color.white);
		Debug.DrawLine(bounds.topRight,   bounds.topLeft,     Color.white);

		for (int i = 0; i < bounds.vRayCount; i++)
		{
			Vector2 shift = new Vector2(i*bounds.vRaySpace, 0);
			Debug.DrawRay(bounds.bottomLeft+shift, -Vector2.up*0.25f, collision.below?Color.blue:Color.red);
			Debug.DrawRay(bounds.topLeft+shift,     Vector2.up*0.25f, collision.above?Color.blue:Color.red);
		}	

		for (int i = 0; i < bounds.hRayCount; i++)
		{
			Vector2 shift = new Vector2(0, i*bounds.hRaySpace);
			Debug.DrawRay(bounds.bottomLeft+shift, -Vector2.right*0.25f, collision.left?Color.blue:Color.red);
			Debug.DrawRay(bounds.bottomRight+shift, Vector2.right*0.25f, collision.right?Color.blue:Color.red);
		}	
	}

	public void Move(Vector3 velocity) 
	{
		collision.reset();
		bounds.reload(cc);
		VerticalCollisions(ref velocity); 
		HorizontalCollisions(ref velocity);

		if (DebugFlag) {DebugCollider();}
		transform.Translate(velocity);
	}

	public void VerticalCollisions(ref Vector3 velocity)
	{
		if (velocity.y == 0) { return; }
		
		float direction = Mathf.Sign(velocity.y);
		float speed = Mathf.Abs(velocity.y);

		for (int i = 1; i < bounds.vRayCount-1; i++)
		{
			Vector2 origin = bounds.bottomLeft;
			if (direction == 1) 
			{
				origin = bounds.topLeft;
			}
			Vector2 shift = new Vector2(i*bounds.vRaySpace, 0);
			
			RaycastHit2D hit = Physics2D.Raycast(origin+shift, direction*Vector2.up, speed, collisionMask);
			if (DebugFlag) { Debug.DrawRay(origin+shift, direction*Vector2.up, Color.green); }

			if (hit.collider != null) 
			{
				velocity.y = hit.distance * direction;
				speed = hit.distance;

				collision.above = direction ==  1;
				collision.below = direction == -1;
			}
		}
	}

	public void HorizontalCollisions(ref Vector3 velocity)
	{
		if (velocity.x == 0) { return; }

		float direction = Mathf.Sign(velocity.x);
		float speed = Mathf.Abs(velocity.x);

		for (int i = 1; i < bounds.hRayCount-1; i++)
		{
			Vector2 origin = bounds.bottomLeft;
			if (direction == 1) 
			{
				origin = bounds.bottomRight;
			}
			Vector2 shift = new Vector2(0, i*bounds.hRaySpace);
			
			RaycastHit2D hit = Physics2D.Raycast(origin+shift, direction*Vector2.right, speed, collisionMask);
			if (DebugFlag) { Debug.DrawRay(origin+shift, direction*Vector2.right, Color.green); }

			if (hit.collider != null) 
			{
				Debug.Log("Hit");
				velocity.x = hit.distance * direction;
				speed = hit.distance;

				collision.right = direction ==  1;
				collision.left  = direction == -1;
			}
		}
	}

	public struct ControllerBounds 
	{
		public Vector2 topLeft, topRight, bottomLeft, bottomRight;


		public int hRayCount, vRayCount;
		public float hRaySpace, vRaySpace;
		
		public void initRay(BoxCollider2D cc) 
		{
			Bounds b = cc.bounds;
			
			// Make sure we have at least two rays
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
