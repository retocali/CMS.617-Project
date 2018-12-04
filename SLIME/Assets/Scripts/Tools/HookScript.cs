using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 [RequireComponent(typeof(Controller2D))]
public class HookScript : MonoBehaviour, ToolsInterface
 {
	 public AudioClip releaseSound;
	 private AudioSource audsrc;
 	 private bool hooked = false;
 	 private bool moved = false;
 	 private GameObject player;
 	 private int gapTime = 0;
 	 public float elasticity = 50.0f;
	 private int timeToRelease = 5;
 	 private float y; 
 	 private float x;

	  private Vector3 playerPos;

     private Vector3 posI;

	 public GameObject indicator;

	 private GameObject aim;
 	// Use this for initialization
	void Start () {
		y = gameObject.transform.position.y;
		x = gameObject.transform.position.x;
		audsrc = GetComponent<AudioSource>();
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
				if (playerPos.y >= y - 0.5) {
					playerPos += new Vector3(0, -0.01f, 0);
					player.transform.Translate(0, -0.01f, 0);
				}	
			}
			if (input.y == 1) {
				moved = true;
				if (playerPos.y <= y + 0.5) {
					playerPos += new Vector3(0, 0.01f, 0);
					player.transform.Translate(0, 0.01f, 0);
				}	
			}
			if (input.x == -1) {
				moved = true;
				if (playerPos.x >= x - 0.5 ){
					playerPos += new Vector3(-0.01f, 0, 0);
					player.transform.Translate(-0.01f, 0, 0);
				}	
			}
			if (input.x == 1) {
				moved = true;
				if (playerPos.x <= x + 0.5 ){
					playerPos += new Vector3(0.01f, 0, 0);
					player.transform.Translate(0.01f, 0, 0);
				}	
			}
			else if (input.x == 0 && input.y == 0 && moved || player.GetComponent<PlayerScript>().IsDead()) {
				// player.transform.position = Vector3.MoveTowards(player.transform.position, 
				// 												gameObject.transform.position, 0.05f);
				audsrc.PlayOneShot(releaseSound);
				Release();
				moved = false;
				return;
 			}


			var diff = gameObject.transform.position - playerPos;
			Debug.Log(diff);

			var rend = gameObject.GetComponent<LineRenderer>();

			rend.SetPosition(0,new Vector3(0,0,0));
			rend.SetPosition(1, new Vector3(diff.y * 2.5f, -diff.x *2.5f, 0));

			
			if (player.transform.position.x > x) {
				posI += new Vector3(diff.x, 0, 0);
			}

			if (player.transform.position.x < x) {
				posI += new Vector3(-diff.x, 0, 0);
			}
			if (player.transform.position.y > y) {
				posI += new Vector3(0, -diff.y, 0);
			}
			if (player.transform.position.y < y) {
				posI += new Vector3(0, diff.y, 0);
			}
			// Destroy(aim);
			// aim = (GameObject)Instantiate (
			// indicator,
			// gameObject.transform.position + diff,
			// new Quaternion(0,0,0,0));
		}
 			
			
		
 		player.GetComponent<PlayerScript>().MultiplyVelocity(0);
	}
	private void Release()
	{	
		var rend = gameObject.GetComponent<LineRenderer>();
		rend.SetPosition(0, new Vector3(0,0,0));
		rend.SetPosition(1, new Vector3(0,0,0));
		var diff = gameObject.transform.position - playerPos;
		var pX = Math.Abs(diff.x) * elasticity;
		var pY = Math.Abs(diff.y) * elasticity;
 		if (player.transform.position.x > x) {
			pX *= -1;
		}
		if (player.transform.position.y > y) {
			pY *= -1;
		}
 		var center = new Vector3(pX, pY, 0).normalized * 2.4f;

		// if (pY < 0) {
		// 	player.transform.position += new Vector3(0, -1.2f, 0);
		// }
		// else {
		// 	player.transform.position += new Vector3(0, 1.2f, 0);
		// }
		// if (pX < 0) {
		// 	player.transform.position += new Vector3(-1.2f, 0, 0);
		// }
		// else {
		// 	player.transform.position += new Vector3(1.2f, 0, 0);
		// }

		player.transform.position += center;

		
		player.GetComponent<PlayerScript>().AddVelocity(new Vector3(pX, pY, 0));
		hooked = false;
 	}
	public void Interact(GameObject p)
	{
		// if (Input.GetAxisRaw("Jump") == 0 
		// || p.GetComponent<PlayerScript>().IsDead()) {
		// 	return;
		// }
		if (p.GetComponent<PlayerScript>().IsDead()) {
			return;
		}
		

		player = p;
		player.GetComponent<PlayerScript>().MultiplyVelocity(0);
		posI = transform.position;
		playerPos = gameObject.transform.position;
		hooked = true;
		gapTime = timeToRelease;
		p.transform.position = gameObject.transform.position;
		player.GetComponent<PlayerScript>().UnStun();
	}
}