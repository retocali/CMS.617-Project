using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{

	private Rigidbody2D rb;
	private Vector3 normalScale;
	private float maxDistanceToGround = 0.1f;
	private float defaultGravity;
	private bool dead = false;

	public float maxSpeed = 10.0f;
	public float minSpeed = 0.1f;
	public float acceleration = 10.0f;
	public float deceleration = 0.5f;
	public float jump = 10.0f;
	public float minSquish = 0.5f;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		defaultGravity = rb.gravityScale;
		normalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		if (!dead) 
		{
			Move(h);
			Bounce(v);
			Squish(rb.velocity.y);	
		}
		Debug.Log(rb.velocity.x);
	}
	public void KillPlayer()
	{
		Debug.Log("Player is dead");
		dead = true;
	}

	private bool IsGrounded() 
	{
		var hit = Physics2D.CircleCast(transform.position, transform.localScale.y/2f , -Vector2.up, maxDistanceToGround);
		return hit.collider != null;
	}
	private void Move(float h)
	{
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

	private void Bounce(float v) 
	{
		if (v == 1f) 
		{
			rb.gravityScale -= defaultGravity*Time.deltaTime;
			rb.gravityScale = Mathf.Max(rb.gravityScale, 0.5f*defaultGravity);
		}
		if (v == -1f)
		{
			rb.gravityScale += defaultGravity*Time.deltaTime;
			rb.gravityScale = Mathf.Min(rb.gravityScale, 2.0f*defaultGravity);
		}
		if (v == 0f)
		{
			rb.gravityScale = Mathf.MoveTowards(rb.gravityScale, defaultGravity, jump*defaultGravity*Time.deltaTime);
		}

		if (IsGrounded()) 
		{
			rb.velocity = new Vector2(rb.velocity.x, jump);
		}
	}
	private void Squish(float final) 
	{
		float XScale = transform.localScale.x;
		float YScale = transform.localScale.y;
		float ZScale = transform.localScale.z;

		XScale = normalScale.x*Mathf.Abs(final/jump);

		XScale = Mathf.Max(XScale, minSquish);
		transform.localScale = new Vector3(XScale, YScale, ZScale);
	}
}
