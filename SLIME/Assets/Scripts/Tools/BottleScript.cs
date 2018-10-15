using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BottleScript : MonoBehaviour, ToolsInterface 
{

	private GameObject player;
	private PlayerScript ps;
	private Controller2D myC2D;

	private Vector3 velocity;
	
	private float gravity = -500f;
	private float maxSpeedX = 15f;
	private float maxSpeedY = 500f;
	private float acceleration = 10f;
	
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
		myC2D = GetComponent<Controller2D>();
		defaultColor = GetComponent<MeshRenderer>().material.color;	
	}
	// Update is called once per frame
	void Update () {
		CollisionClamping(ref velocity);
		
		velocity.y = gravity * Time.deltaTime;

		if (ps != null) { WithPlayer(ref velocity);	}
		if (colorCount > 0) { RevertColor(); }
		
		ClampSpeeds(ref velocity);
		myC2D.Move(velocity*Time.deltaTime);	
		
		if (player != null) { player.transform.position = transform.position; }
	}
	private void RevertColor()
	{
		colorCount -= Time.deltaTime;
		if (colorCount <= 0)
		{
			GetComponent<MeshRenderer>().material.color = defaultColor;	
		}
	}
	private void CollisionClamping(ref Vector3 velocity)
	{
		if (myC2D.collision.above || myC2D.collision.below) 
		{
			velocity.y = 0;
		}
		if (myC2D.collision.right || myC2D.collision.left) 
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

		if (input.z == 1 && gapTime <= 0 && !myC2D.collision.above)
		{
			Release();
			return;
		}

		ps.MultiplyVelocity(0);
		ps.AddVelocity(ps.Velocity());
		velocity.x += input.x*acceleration*Time.deltaTime;
	}
	private void ClampSpeeds(ref Vector3 velocity)
	{
		velocity.x = Mathf.Max(Mathf.Min(maxSpeedX, velocity.x), -maxSpeedX);
		velocity.y = Mathf.Max(Mathf.Min(maxSpeedY, velocity.y), -maxSpeedY);
	}

	private void Release()
	{	
		float clearance = player.transform.lossyScale.y;
		if (myC2D.VerticalRaycast(clearance).collider != null) 
		{
			Debug.Log("Cannot release");
			GetComponent<MeshRenderer>().material.color = new Color(0, 0.5f, 1f, 0.5f);
			colorCount = colorDuration;
			return;
		}

		player.transform.position += new Vector3(0, 1.26f, 0);
		ps.AddVelocity(new Vector3(0, 1, 0)*shoot);
		velocity.x = 0;
		ps = null;
		player = null;
	}
	public void Interact(GameObject p)
	{
        if (Input.GetAxisRaw("Jump") == 0) {
			return;
		}
		gapTime = timeToRelease;
		player = p;
		ps = p.GetComponent<PlayerScript>();
	}
}
