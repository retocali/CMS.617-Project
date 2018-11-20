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

	// Use this for initialization
	new void Start () 
	{
		base.Start();
		gravity = -20f;

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
			if (spawned) {
				Debug.Log("Bleh");
				Destroy(gameObject, 0.25f);
			} else {	 
				StartCoroutine(RemoveSelf());	
			}
		}

		base.Update();
	}

	private IEnumerator RemoveSelf()
	{
		dead = true;
		falling = false;
		yield return new WaitForSeconds(0.25f);
		c2d.collision.below = false;
		gameObject.SetActive(false);
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
