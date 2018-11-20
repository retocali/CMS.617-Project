using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SplitterScript : MonoBehaviour, ToolsInterface {

	private bool split = false;

 	private GameObject player;
	private GameObject player2;
	
	public enum Direction {Top, Bottom, Right, Left};

	public Direction outDirection1 = Direction.Bottom;
	public Direction outDirection2 = Direction.Right;

 	private float y; 
 	private float x;
	private float width;
	private float height;

	private float waitTime = 1f;
	private float time = 0f;
 	
	private BoxCollider2D cc;

	 // Use this for initialization
	void Start () {
		y = transform.position.y;
		x = transform.position.x;

		cc = GetComponent<BoxCollider2D>();
		
		width  = cc.size.x;
		height = cc.size.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (player2 != null) {
			if ( player2.GetComponent<PlayerScript>().IsDead()) {
				resetSplitter();
				player.GetComponent<PlayerScript>().KillPlayer();
			}
		}
		if (split && time < waitTime) 
		{
			time += Time.deltaTime;
			if (time > waitTime) {
				Split();
			} else {
				Hold();
			}
		}


	}
	public void Interact(GameObject p)
	{
		if (!split) {
			player = p;
			
			split = true;
		}

		
	}
	private void Split() 
	{
		float w = player.transform.localScale.x/2+0.1f+width/2;
		float l = player.transform.localScale.y/2+0.1f+height/2;
		Vector2 distance = new Vector2(w, l);
		Vector3 origin   = new Vector3(x, y, 0);

		player.transform.position = ApplyDirection(origin, distance, outDirection1);
		player2 = Instantiate(player, ApplyDirection(origin, distance, outDirection2), Quaternion.identity);
		player2.GetComponent<PlayerScript>().duplicateSlime();
	}

	private void Hold() 
	{
		player.transform.position = transform.position;
		player.GetComponent<PlayerScript>().MultiplyVelocity(0);
	}
	
	private Vector3 ApplyDirection(Vector3 origin, Vector2 distance, Direction d)
	{
		switch (d)
		{
			case Direction.Top:
				return origin + new Vector3(0, distance.y, 0);
			case Direction.Bottom:
				return origin + new Vector3(0, -distance.y, 0);
			case Direction.Left:
				return origin + new Vector3(-distance.x, 0, 0);
			case Direction.Right:
				return origin + new Vector3(distance.x, 0, 0);
			default:
				Debug.LogError("SplitterScript: Direction " + d + " does not exist");
				return origin;
		}
	}

	public void resetSplitter() {
		if (player == null)
		{
			return;
		}
		Destroy(player2);
		split = false;
		time = 0.1f;

	}

	public bool isSplit() {
		return split;
	}
}

