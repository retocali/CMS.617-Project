using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 [RequireComponent(typeof(Controller2D))]
public class HookScript : MonoBehaviour, ToolsInterface
 {
 	 private bool hooked = false;
 	 private bool moved = false;
 	 private GameObject player;
 	 private int gapTime = 0;
 	 public float elasticity = 50.0f;
	 private int timeToRelease = 5;
 	 private float y; 
 	 private float x;
 	// Use this for initialization
	void Start () {
		y = gameObject.transform.position.y;
		x = gameObject.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (gapTime > 0) { gapTime -= 1; }
		if (hooked) {
 			WithPlayer();
		}
	}
 	private void WithPlayer()
	{
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 
									Input.GetAxisRaw("Vertical"),
									Input.GetAxisRaw("Jump"));
 		if (gapTime <= 0)
		{
			if (input.y == -1) {
				moved = true;
				if (player.transform.position.y >= y - 0.5) {
					player.transform.Translate(0, -0.01f, 0);
				}	
			}
			if (input.y == 1) {
				moved = true;
				if (player.transform.position.y <= y + 0.5) {
					player.transform.Translate(0, 0.01f, 0);
				}	
			}
			if (input.x == -1) {
				moved = true;
				if (player.transform.position.x >= x - 0.5 ){
					player.transform.Translate(-0.01f, 0, 0);
				}	
			}
			if (input.x == 1) {
				moved = true;
				if (player.transform.position.x <= x + 0.5 ){
					player.transform.Translate(0.01f, 0, 0);
				}	
			}
			else if (input.x == 0 && input.y == 0 && moved) {
				// player.transform.position = Vector3.MoveTowards(player.transform.position, 
				// 												gameObject.transform.position, 0.05f);
				Release();
				moved = false;
				return;
 			}
		}
 			
			
		
 		player.GetComponent<PlayerScript>().MultiplyVelocity(0);
	}
	private void Release()
	{	
		var diff = gameObject.transform.position - player.transform.position;
		var pX = Math.Abs(diff.x) * elasticity;
		var pY = Math.Abs(diff.y) * elasticity;
 		if (player.transform.position.x >= x) {
			pX *= -1;
		}
		if (player.transform.position.y >= y) {
			pY *= -1;
		}
 		player.transform.position = transform.position;
		if (pY < 0) {
			player.transform.position += new Vector3(0, -1.2f, 0);
		}
		else {
			player.transform.position += new Vector3(0, 1.2f, 0);
		}
		
		player.GetComponent<PlayerScript>().AddVelocity(new Vector3(pX, pY, 0));
		hooked = false;
 	}
	public void Interact(GameObject p)
	{
		if (Input.GetAxisRaw("Jump") == 0 ) {
			return;
		}
		player = p;
		hooked = true;
		gapTime = timeToRelease;
		p.transform.position = gameObject.transform.position;
		player.GetComponent<PlayerScript>().MultiplyVelocity(0);
		
	}
}