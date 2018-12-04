using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaverScript : EnemyClass
{

	public Curve[] parameters;
	public AudioClip switchSound;

	private int i;
	private float degrees;

	[System.Serializable]
	public struct Curve {
		public float width;
		public float height;
		public float totalDegrees;
		public float startDegrees;
		public float step;
	}
	// Use this for initialization
	new void Start () 
	{
		i = 0;
		if (parameters.Length == 0) {
			Warn("Parameters must not be empty, use a spike instead");
			functional = false;
			
		} else {
			degrees = parameters[0].startDegrees;
		}
		base.Start();
	}
	
	public override void Respawn()
	{
		i = 0;
		if (functional)
			degrees = parameters[0].startDegrees;
			
		base.Respawn();
	}


	// Update is called once per frame
	new	void Update () 
	{
		if (!functional) { 
			Warn(transform.name+" not functional"); 
			return; 
		}
		Curve p = parameters[i];
		degrees += p.step;
		if (degrees >= p.totalDegrees) 
		{
			i = (i+1) % parameters.Length;
			degrees = parameters[i].startDegrees;
			audsrc.PlayOneShot(switchSound, 0.5f);
		}
		
		float t = degrees*Mathf.Deg2Rad;

		velocity.x = p.width  * p.step * Mathf.Cos(t);
		velocity.y = p.height * p.step * Mathf.Sin(t);

		base.Update();
	}
}
