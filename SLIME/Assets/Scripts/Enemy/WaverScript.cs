using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaverScript : EnemyClass
{

	public Curve[] parameters;
	public float[] times;

	private int i;
	private float currentTime;
	private float speed = 10f;

	[System.Serializable]
	public struct Curve {
		public float degree;
		public float amplitude;
		public float speed;
	}
	// Use this for initialization
	new void Start () 
	{
		i = 0;
		currentTime = 0;
		if (parameters.Length == 0 || times.Length == 0) {
			Warn("Directions and times must not be empty");
		}
		if (parameters.Length != times.Length) {
			Warn("Directions and times must be the same length!");
		}
		base.Start();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (!functional) { 
			Warn(transform.name+" not functional"); 
			return; 
		}
		currentTime += Time.deltaTime;
		if (currentTime >= times[i]) 
		{
			currentTime = 0;
			i = (i+1) % parameters.Length;
		}

		Debug.Log(currentTime);
		// float A = parameters[i].x;
		// float f = parameters[i].y;
		// float p = parameters[i].z;
		
		float t = (currentTime/times[i])*2*Mathf.PI;

		transform.position = initialLoc+speed*new Vector3(Mathf.Cos(t), Mathf.Sin(t), 0);
		
		// base.Update();
	}
}
