using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BottleScript : MonoBehaviour, ToolsInterface 
{
	public bool weaponized = false;

	private GameObject player;
	private PlayerScript ps;
	private Controller2D c2d;

	private Vector3 velocity;
	
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
		defaultColor = transform.GetChild(0).GetComponent<SpriteRenderer>().material.color;	
		initialLoc = transform.position;
		ReloadMaster.AddToMaster(this);
	}
	// Update is called once per frame
	void Update () {
		CollisionClamping(ref velocity);
		
		velocity.y += gravity * Time.deltaTime;

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

	private void Release()
	{	
		float clearance = player.transform.lossyScale.y;
		if (c2d.VerticalRaycast(clearance).collider != null) 
		{
			Debug.Log("Cannot release");
			transform.GetChild(0).GetComponent<SpriteRenderer>().material.color = new Color(0, 0.5f, 1f, 0.5f);
			colorCount = colorDuration;
			return;
		}

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
		if (ps.IsDead()) {
			ps = null;
			return;
		}
		gapTime = timeToRelease;
		player = p;
	}
	public void Respawn()
	{
		transform.GetChild(0).Rotate(-1*transform.GetChild(0).eulerAngles);
		transform.position = initialLoc;
   		velocity = Vector3.zero;
	}

}
