using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperScript : EnemyClass 
{	
	public bool sensitive = true;
	private bool falling = false;
	private Transform player;

    private Animator animor;

	private float XRange = 4f;
	private float YRange = 0f;
	private float time = 1f;

	// Use this for initialization
	new void Start () 
	{
		base.Start();
		gravity = -20f;
		if (spawned) {time = 0;}
        animor = GetComponent<Animator>();
        

	}
	
	// Update is called once per frame
	new void Update () 
	{	
		if (!falling && !PlayerInRange()) 
		{
			return;
		}
		falling = true;
		
		ApplyGravity(ref velocity, Time.deltaTime);
        animor.SetTrigger("drop");

        if (c2d.collision.below) 
		{
			dead = true;
			if (spawned) {
				Destroy(gameObject, 0.25f);
			} else {	 
				StartCoroutine(RemoveSelf());	
			}
		}
		base.Update();
	}

	private IEnumerator RemoveSelf()
	{
		yield return new WaitForSeconds(0.25f);
		if (dead) {
			gameObject.SetActive(false);	
		}
	}

	public override void Respawn()
	{
		falling = false;
		c2d.collision.below = false;
		time = 1f;
		base.Respawn();
	}
	
	private bool PlayerInRange()
	{
		if (time > 0) {
			time -= Time.deltaTime;
			return false;
		}

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
