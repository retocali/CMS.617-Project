using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BottleScript : MonoBehaviour, ToolsInterface 
{
	public bool broke = false;

	public AudioClip outSound;
	public AudioClip inSound;
	private AudioSource audsrc;

	private GameObject player;
	private PlayerScript ps = null;
	private Controller2D c2d;
	private ParticleSystem partSys;

	private Vector3 velocity;
	private Vector3 addedVelocity;


	private const float gravity = -20f;
	private const float minSpeedX = 0.25f;
	private const float maxSpeedX = 15f;
	private const float maxSpeedY = 500f;
	private const float acceleration = 15f;
	private const float deceleration = 0.95f;
	private const float minSpeedToZero = 0.05f;
	private Vector3 initialLoc;
	
	private float shoot = 10;
	private float gapTime = 0;
	private float timeToRelease = 0.5f;
	
	private Color defaultColor;
	private float colorCount = 0;
	private float colorDuration = 0.25f;

	void Start() {
		ps = null;	
		player = null;
		velocity = Vector3.zero;
		c2d = GetComponent<Controller2D>();
		partSys = GetComponent<ParticleSystem>();
		audsrc = GetComponent<AudioSource>();
		defaultColor = transform.GetChild(0).GetComponent<SpriteRenderer>().material.color;	
		initialLoc = transform.position;
		ReloadMaster.AddToMaster(this);
	}
	// Update is called once per frame
	void Update () {
		CollisionClamping(ref velocity);
		
		velocity.y += gravity * Time.deltaTime;
        velocity += addedVelocity;
        addedVelocity = Vector3.zero;

		if (ps != null) { 
			WithPlayer(ref velocity);	
		} else {
			velocity.x *= deceleration;
			if (Mathf.Abs(velocity.x) < minSpeedToZero)
				velocity.x = 0;
		}
		if (colorCount > 0) { RevertColor(); }
		
		ClampSpeeds(ref velocity);
		c2d.Move(velocity*Time.deltaTime);	
		transform.GetChild(0).Rotate(new Vector3(0,0,-1)*velocity.x);
		if (player != null) { player.transform.position = transform.position; }
	}
	private void RevertColor()
	{
		colorCount -= Time.deltaTime;
		if (colorCount <= 0)
		{
			transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = defaultColor;	
		}
	}
	private void CollisionClamping(ref Vector3 velocity)
	{
		if (c2d.collision.above || c2d.collision.below) 
		{
			velocity.y = 0;
		}
		if (c2d.collision.right || c2d.collision.left) 
		{
			velocity.x = 0;
		}
	}

	private void WithPlayer(ref Vector3 velocity)
	{
		if (gapTime > 0) { gapTime -= Time.deltaTime; }
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 
									0,
									Input.GetAxisRaw("Jump"));

		if (input.z == 1 && gapTime <= 0 && !c2d.collision.above || ps.IsDead())
		{
			Release();
			return;
		} 
		ps.DestroyCrumbs(0.25f);
		ps.MultiplyVelocity(0);
		ps.AddVelocity(ps.Velocity());
		velocity.x += input.x*acceleration*Time.deltaTime;
	}
	private void ClampSpeeds(ref Vector3 velocity)
	{
		velocity.x = Mathf.Max(Mathf.Min(maxSpeedX, velocity.x), -maxSpeedX);
		velocity.y = Mathf.Max(Mathf.Min(maxSpeedY, velocity.y), -maxSpeedY);
		if (Mathf.Abs(velocity.x) < minSpeedX) {
			velocity.x = 0;
		}
	}
	public void Break()
	{
		if (broke)
		{
			// Dont fix it
			return;
		}
		broke = true;
		Debug.Log("Break");
		Release(true);
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Controller2D>().enabled = false;
		partSys.Play();
		transform.GetChild(0).gameObject.SetActive(false);
		GetComponent<BottleScript>().enabled = false;
	}

	private void Release(bool force = false)
	{	
		if(ps == null) {
			return;
		}
		float clearance = player.transform.lossyScale.y;
		if (c2d.VerticalRaycast(clearance).collider != null && !force) 
		{
			Debug.Log("Cannot release");
			transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = new Color(0, 0.5f, 1f, 0.5f);
			colorCount = colorDuration;
			return;
		}
		audsrc.PlayOneShot(outSound);
		player.transform.position += new Vector3(0, 1.26f, 0);
		ps.AddVelocity(new Vector3(0, 1, 0)*shoot);
		ps = null;
		player = null;
	}
	public void Interact(GameObject p)
	{
        if (Input.GetAxisRaw("Jump") == 0) {
			return;
		}
		ps = p.GetComponent<PlayerScript>();
		ps.UnStun();
		if (ps.IsDead()) {
			ps = null;
			return;
		}
		if (player == null) { audsrc.PlayOneShot(inSound); }
		gapTime = timeToRelease;
		player = p;
	}
	public void Respawn()
	{
		transform.GetChild(0).Rotate(-1*transform.GetChild(0).eulerAngles);
		transform.position = initialLoc;
   		velocity = Vector3.zero;
	}

    /**
    A method used for adding velocity to the bottle
    negative numbers will decrease speed. Accumulates
    */
    public void AddVelocity(Vector3 v)
    {
        addedVelocity += v;
    }

}
