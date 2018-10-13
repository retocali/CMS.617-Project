using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperScript : EnemyClass 
{	
	public bool sensitive = true;
	private bool falling = false;
	private Transform player;

	private float XRange = 4f;
	private float YRange = 0f;

	// Use this for initialization
	void Start () 
	{
		base.Start();
		gravity = -20f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!falling && !PlayerInRange()) 
		{
			return;
		}
		falling = true;
		ApplyGravity(ref velocity, Time.deltaTime);

		if (c2d.collision.below) 
		{ 
			Destroy(gameObject, 0.25f);
		}

		base.Update();
	}

	private bool PlayerInRange()
	{
		if (!sensitive) 
		{ 
			return true; 
		}

		if (player == null) 
		{ 
			player = PlayerScript.FindPlayer().transform; 
		}

		Vector3 delta = player.position-transform.position;
		return (Mathf.Abs(delta.x) < XRange && (delta.y) < YRange);
	}

}
