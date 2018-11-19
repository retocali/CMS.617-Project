using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallScript : MonoBehaviour {

	private GameObject player;

	private float fireSpeed;

	// Use this for initialization
	void Start () {

        // Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
 
        // // Aim bullet in player's direction.
        // transform.rotation = Quaternion.LookRotation(playerPos);
		
	}
	
	// Update is called once per frame
	void Update () {
		 // Move the projectile forward towards the player's last known direction;
        // transform.position += transform.forward * fireSpeed;
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player") {
			other.gameObject.GetComponent<PlayerScript>().KillPlayer();
			Destroy(gameObject);
		}
	}

	// public void SetPlayerPos(GameObject p) {
	// 	player = p;
	// }

	// public void SetSpeed(float s) {
	// 	fireSpeed = s;
	// }
}
