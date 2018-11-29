using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour {

	private Rigidbody2D rb;
	
	public float xAcceleration = 0;
	public float yAcceleration = 0;
	public float yVelocity=10;
	public float xVelocity=0;
	///after cycles frames the direction switches
	public uint cycles = 100;
	public uint cycleOffset= 50;
	///number of frames to wait after completing a cycle to switching direction
	public uint turnAroundDelay=100;
	private uint delayCounter=0;
	// Use this for initialization
	void Start () {
		
		rb = GetComponent<Rigidbody2D>();
		resetVelocity();
	}
	
	// Update is called once per frame
	void Update () {
		if(delayCounter!=0){
			delayCounter--;
			if(delayCounter==0)
				resetVelocity();
			return;
		}
		cycleOffset++;
		if(cycleOffset%cycles ==0){
			yVelocity=-yVelocity;
			xVelocity=-xVelocity;
			xAcceleration=-xAcceleration;
			yAcceleration=-yAcceleration;
			if(turnAroundDelay>0)
				rb.velocity = new Vector2(0,0);
			else resetVelocity();
			delayCounter=turnAroundDelay;
			return;
		}
		// Debug.Log(rb.velocity);
		// Debug.Log(rb.position);
		float deltaT=Time.deltaTime;
		rb.velocity += new Vector2(deltaT*xAcceleration, deltaT*yAcceleration);
	}
	void resetVelocity(){
		///should save this value so we don't have to recreate it
		rb.velocity = new Vector2(xVelocity,yVelocity);
	}
}
