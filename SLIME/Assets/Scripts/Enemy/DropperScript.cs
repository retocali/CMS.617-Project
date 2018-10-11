using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class DropperScript : MonoBehaviour {
	public bool sensitive = true;
	
	private Vector3 velocity;
	private Vector2 addedVelocity;
	private Controller2D c2d;
	private float gravity = -20f;
	private bool falling = false;
	private Transform player;

	private float XRange = 1;
	private float YRange = 1;

	// Use this for initialization
	void Start () {
		velocity = Vector3.zero;
		c2d  = GetComponent<Controller2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!PlayerInRange() && !falling)
		{
			return;
		}

		falling = true;
		
		velocity.y += addedVelocity.y;
		velocity.x += addedVelocity.x;
		addedVelocity = Vector2.zero;

		velocity.y += Time.deltaTime*gravity;
		if (c2d.collision.below) {
			Destroy(gameObject, 0.25f);
		}
		if (c2d.collision.above) {
			velocity.y = 0;
		}
		if (c2d.collision.right || c2d.collision.left) {
			velocity.x = 0;
		}

		c2d.Move(velocity*Time.deltaTime);
	}

	private bool PlayerInRange()
	{
		if (!sensitive) { return true; }
		if (player == null)
		{
			player = PlayerScript.FindPlayer().transform;
		}

		return (Mathf.Abs(player.position.x-transform.position.x) < XRange
					  && (player.position.y-transform.position.y) < YRange);
	}

	public void AddVelocity(Vector2 v)
	{
		addedVelocity += v;
	}
}
