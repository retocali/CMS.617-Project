using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private float lastShot;

	public float fireRate;

	public float fireSpeed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time > lastShot + (fireRate )){
			lastShot = Time.time;
			Fire();
		}
	}
	

	void Fire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * -fireSpeed;

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}
}
