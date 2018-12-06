using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossScript : MonoBehaviour {
    
    public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private float lastShot;

	public float fireRate;

	public float fireSpeed;
	public float spread;
	public GameObject player;

	public GameObject partSys;

	public int health = 3;

	private float timer = 0;
	private const float gapTime = 0.5f;

	private CameraScript camera;

	private Animator animator;

	public static bool shouldShoot;

	private Vector3 playerStart;

    public AudioClip roar;

	private bool dead = false;


	// Use this for initialization
	void Start () {
		shouldShoot = false;
		lastShot = -fireRate;
		animator = GetComponent<Animator>();
		camera = Camera.main.gameObject.GetComponent<CameraScript>();
		playerStart = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		if( !dead) {
			if (Time.time > lastShot + (fireRate) + Random.value * fireRate && FinalBossGateManager.hasShownBoss && shouldShoot){
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
	}
	

	void Fire(){
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);
			Vector3 delta = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
			Vector3 direction = (player.transform.position - transform.position + delta*spread).normalized * fireSpeed;

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
		StartCoroutine("showBossHurt");
		if (timer > 0) 
		{	
			return;
		}
		Debug.Log("OUCH");
		bs.Break();
		health--;
		timer = gapTime;
		
		if (health == 0){
			Die();
		}else{
			animator.SetTrigger("damage");
		}
	}
	private void Die(){
		dead = true;
		Debug.Log("Death");
		GameObject particles = Instantiate (
			partSys,
			bulletSpawn.position,
			bulletSpawn.rotation);
		Debug.Log(particles);
		particles.gameObject.GetComponent<ExplodingBossScript>().Explode();
		// particles.GetComponent<ExplodingBossScript>().Explode();
		animator.SetTrigger("kill");
		animator.SetBool("isded", true);
		StartCoroutine("showBossDying");
		player.transform.position = playerStart;
	}

	IEnumerator showBossHurt(){
		shouldShoot = false;
		DestroyAllObjectsWithTag("fireball");
		camera.CameraFocusTimed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 2);
        gameObject.GetComponent<AudioSource>().PlayOneShot(roar);
		yield return new WaitForSeconds(2);
		shouldShoot = true;

	}

	void DestroyAllObjectsWithTag(string tag)
	{
		var gameObjects = GameObject.FindGameObjectsWithTag (tag);
		
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}
	}

	IEnumerator showBossDying(){
		camera.CameraFocusTimed(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), 4);
        gameObject.GetComponent<AudioSource>().PlayOneShot(roar);
        yield return new WaitForSeconds(4);
		// Destroy(gameObject, 0.5f);
		gameObject.SetActive(false);
	}
}
