using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaverScript : EnemyClass
{

	public Curve[] parameters;

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
		}
		degrees = parameters[0].startDegrees;
		base.Start();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
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
		}
		
		float t = degrees*Mathf.Deg2Rad;

		velocity.x = p.width  * p.step * Mathf.Cos(t);
		velocity.y = p.height * p.step * Mathf.Sin(t);

		base.Update();
	}
}
