using System.Collections;
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
	private float timer = 0;
	private const float gapTime = 0.5f;

	private Animator animator;

	private CheckpointMaster checkpointMaster;

	// Use this for initialization
	void Start () {
		lastShot = -fireRate;
		animator = GetComponent<Animator>();
		checkpointMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<CheckpointMaster>();
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time > lastShot + (fireRate) + Random.value * fireRate && checkpointMaster.getCurrentCheckpointIndex() > 0){
			lastShot = Time.time;
			Fire();
		}
		if(health == 0){
			Die();
		}
		if (timer > 0) {
			timer -= Time.deltaTime;
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
		animator.SetTrigger("spit");
	}

	public void Reload()
	{
		health = 3;
	}
	public void Hurt(BottleScript bs)
	{
		if (timer > 0) 
		{	
			return;
		}
		Debug.Log("OUCH");
		bs.Break();
		health--;
		timer = gapTime;
		animator.SetTrigger("damage");
	}
	private void Die(){
		animator.SetTrigger("kill");
		animator.SetBool("isded", true);
		Destroy(gameObject, 0.5f);
	}
}
