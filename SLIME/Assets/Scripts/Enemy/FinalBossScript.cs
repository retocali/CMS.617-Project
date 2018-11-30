﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossScript : MonoBehaviour {

public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private float lastShot;

	public float fireRate;

	public float fireSpeed;
	public GameObject player;

	private int health = 3;

	// Use this for initialization
	void Start () {
		lastShot = -fireRate;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time > lastShot + (fireRate )){
			lastShot = Time.time;
			Fire();
		}
		if(health == 0){
			Die();
		}
	}
	

	void Fire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

			Vector3 direction = (player.transform.position - transform.position).normalized * fireSpeed;

			// Add velocity to the bullet
			bullet.GetComponent<Rigidbody2D>().velocity = direction;

			// bullet.GetComponent<FireBallScript>().SetPlayerPos(player);
			// bullet.GetComponent<FireBallScript>().SetSpeed(fireSpeed);

			Destroy(bullet, 10f);
	}

	public void Reload()
	{
		health = 3;
	}
	public void Hurt(BottleScript bs)
	{
		Debug.Log("OUCH");
		bs.Break();
		health--;
	}
	private void Die(){
		Destroy(gameObject);
	}
}
