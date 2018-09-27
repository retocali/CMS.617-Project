﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class urgency : MonoBehaviour {

	private GameObject player;
	public ArrayList list;
	///This item will be at the player's position in time frames
	public float speed=10;
	private int counter=0;
	private Rigidbody2D rb;
	private Vector2 startingPos;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		startingPos=new Vector2(rb.position.x,rb.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		//refactor into seperate method
		if (player == null) 
		{
			player = GameObject.Find("Player(Clone)");
			if (player == null) 
			{
				Debug.Log("Error: Could not find player prefab? Name might have changed");
				return;
			}
			Debug.Log("Resolved: Found player!");
			rb.position=new Vector2(startingPos.x,startingPos.y);
		} 
		rb.position=Vector2.MoveTowards(rb.position,player.transform.position,speed);
	}
}
