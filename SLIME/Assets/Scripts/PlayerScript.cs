using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{

	private Rigidbody2D rb;
	private Vector2 normal = Vector2.zero;
	private Vector3 normalScale;
	private float maxDistanceToGround = 0.25f;
	private float defaultGravity;
	private float defaultJump;
	private bool dead = false;
	private bool touching = false;
	private int layermask = ~(1 << 9 | 1 << 10);
	private GameObject[] crumbs;
	private int crumbIndex = 0;
	private float crumbGap = 0f;

	public bool quantumJump = false;
	public bool quantumMove = false;
	public int scheme = -1;
	public float crumbSpace = 2.0f;
	public int crumbNum = 50;
	public GameObject crumbPrefab;
	public float maxSpeed = 10.0f;
	public float acceleration = 10.0f;
	public float deceleration = 0.5f;
	public float jump = 10.0f;
	public float walljump = 7.5f;
	public float weightDelta = 10.0f;
	public float maxWeight = 2.0f;
	public float minWeight = 0.5f;
	public float maxSquish = 1.5f;
	public float minSquish = 0.5f;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		defaultGravity = rb.gravityScale;
		defaultJump = jump;
		normalScale = transform.localScale;
		switch (scheme)
		{
			default:
				break;
			case 0:
				deceleration = 0.5f;
				acceleration = 50.0f;
			break;
			case 1:
				maxWeight = 2.5f;
				minWeight = 0.45f;
				weightDelta = 100.0f;
			break;
			case 2:
				walljump = 5.0f;
				jump = 15.0f;
			break;
			case 3:
				maxSpeed = 15.0f;
				acceleration = 30.0f;
				weightDelta = 100.0f;
			break;
		}
		crumbs = new GameObject[crumbNum];
	}
	void Trail() {
		crumbGap += crumbNum*Time.deltaTime;
		if (crumbGap < crumbSpace) {
			return;
		}
		crumbGap = 0;
		Destroy(crumbs[crumbIndex]);
		crumbs[crumbIndex] = Instantiate(crumbPrefab,
								new Vector3(transform.position.x,transform.position.y,-6),
								transform.rotation) as GameObject;
		crumbIndex = (crumbIndex + 1) % crumbNum;
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		if (!dead) 
		{
			if (quantumJump) {
				qBounce(v); 
			} else {
				Bounce(v);
			}
			if (quantumMove) {
				qMove(h); 
			} else {
				Move(h);
			}
			Squish(rb.velocity.y);	
			Trail();
		} 
		else 
		{
			float XScale = transform.localScale.x;
			float YScale = transform.localScale.y;
			float ZScale = transform.localScale.z;

			XScale = Mathf.Min(XScale*1.05f, 5f);
			YScale = Mathf.Max(YScale*0.8f, 0.0f);
			transform.localScale = new Vector3(XScale, YScale, ZScale);
		}
	}
	private void Touch(Collision2D c)
	{
		touching = true;
		normal = c.GetContact(0).normal;
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "hazard") {
			KillPlayer();
		}
	}
	private void OnCollisionEnter2D(Collision2D other) { Touch(other); }
	private void OnCollisionStay2D(Collision2D other) { Touch(other); }
	private void OnCollisionExit2D(Collision2D other) { touching = false; }
	
	public void KillPlayer()
	{
		Debug.Log("Player is dead");
		dead = true;
		for (int i = 0; i < crumbNum; i++) {
			Destroy(crumbs[i], 1.5f);
		}
		GetComponent<Renderer>().material.color = Color.red;
		Destroy(gameObject, 1.5f);
	}
	private bool IsGrounded() 
	{
		float x = transform.localScale.x*0.5f;
		float y = transform.localScale.y;

		Vector2 scale = new Vector2(x, y);
		var hit = Physics2D.BoxCast(transform.position, scale, 0f, -Vector2.up, maxDistanceToGround, layermask);
		return hit.collider != null;
	}
	private void Move(float h)
	{
		float minSpeed = 0.1f;
		if (h != 0) 
		{
			rb.velocity += new Vector2(Time.deltaTime*h*acceleration, 0);
			if (Mathf.Abs(rb.velocity.x) > maxSpeed) 
			{	
				rb.velocity = new Vector2(maxSpeed*Mathf.Sign(rb.velocity.x), rb.velocity.y);
			}
		} 
		else 
		{
			if (Mathf.Abs(rb.velocity.x) > minSpeed) 
			{
				rb.velocity = new Vector2(rb.velocity.x*deceleration, rb.velocity.y);
			} 
			else 
			{
				rb.velocity = new Vector2(0, rb.velocity.y);
			}
		}
	}
	private void qMove(float h)
	{
		float minSpeed = 0.1f;
		if (h != 0) 
		{
			rb.velocity += new Vector2(h*acceleration, 0);
			if (Mathf.Abs(rb.velocity.x) > maxSpeed) 
			{	
				rb.velocity = new Vector2(maxSpeed*Mathf.Sign(rb.velocity.x), rb.velocity.y);
			}
		} 
		else 
		{
			rb.velocity -= new Vector2(h*acceleration, 0);
			if (Mathf.Abs(rb.velocity.x) < minSpeed) 
			{
				rb.velocity = new Vector2(0, rb.velocity.y);
			}
		}
	}

	private void qBounce(float v) 
	{
		if (v == 1f) 
		{
			jump = maxWeight/1.5f*defaultJump;
		}
		if (v == -1f)
		{
			jump = minWeight/1.5f*defaultJump;
		}
		if (v == 0f)
		{
			jump = defaultJump;
		}

		if (IsGrounded()) 
		{
			rb.velocity = new Vector2(rb.velocity.x, jump);
		}
		else if (touching) 
		{
			rb.velocity = jump*normal;
			float sign = Mathf.Sign(rb.velocity.y);
			if (Mathf.Abs(rb.velocity.x) > 0) {
				sign = 1f;
			}
			if (Input.GetButton("Jump")) {
				rb.velocity = new Vector2(rb.velocity.x, sign*walljump);
			} 
		}
	}

	private void Bounce(float v) 
	{
		if (v == 1f) 
		{
			rb.gravityScale -= weightDelta*Time.deltaTime;
			rb.gravityScale = Mathf.Max(rb.gravityScale, minWeight*defaultGravity);
		}
		if (v == -1f)
		{
			rb.gravityScale += weightDelta*Time.deltaTime;
			rb.gravityScale = Mathf.Min(rb.gravityScale, maxWeight*defaultGravity);
		}
		if (v == 0f)
		{
			rb.gravityScale = Mathf.MoveTowards(rb.gravityScale, defaultGravity, jump*defaultGravity*Time.deltaTime);
		}

		if (IsGrounded()) 
		{
			rb.velocity = new Vector2(rb.velocity.x, jump);
		}
		else if (touching) 
		{
			rb.velocity = jump*normal;
			float sign = Mathf.Sign(rb.velocity.y);
			if (Mathf.Abs(rb.velocity.x) > 0) {
				sign = 1f;
			}
			if (Input.GetButton("Jump")) {
				rb.velocity = new Vector2(rb.velocity.x, sign*walljump);
			} 
		}
	}
	private void Squish(float final) 
	{
		float XScale = transform.localScale.x;
		float YScale = transform.localScale.y;
		float ZScale = transform.localScale.z;

		XScale = normalScale.x*Mathf.Abs(final/jump);

		XScale = Mathf.Max(XScale, minSquish);
		XScale = Mathf.Min(XScale, maxSquish);
		transform.localScale = new Vector3(XScale, YScale, ZScale);
	}
}


