using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour 
{

	private Rigidbody2D rb;
	private float defaultGravity;
	private float maxDistanceToGround = 0.1f;
	public float maxSpeed = 10.0f;
	public float minSpeed = 0.1f;
	public float acceleration = 10.0f;
	public float deceleration = 0.5f;
	public float jump = 10.0f;
	
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		defaultGravity = rb.gravityScale;
	}
	 
	bool IsGrounded() 
	{
		var hit = Physics2D.CircleCast(transform.position, 0.5f, -Vector2.up, maxDistanceToGround);
		return hit.collider != null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		if (h != 0) {
			rb.velocity += new Vector2(Time.deltaTime*h*acceleration, 0);
			if (rb.velocity.x > maxSpeed) 
			{	
				rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
			}
		} else {
			Debug.Log(rb.velocity.x);
			if (Mathf.Abs(rb.velocity.x) > minSpeed) {
				rb.velocity = new Vector2(rb.velocity.x*deceleration, rb.velocity.y);
			} else {
				rb.velocity = new Vector2(0, rb.velocity.y);
			}
		}

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
			rb.gravityScale = defaultGravity;
		}
		
		if (IsGrounded()) 
		{
			rb.velocity = new Vector2(rb.velocity.x, jump);
		}
	}
}
