using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject player;

	private Camera cam;
	private Vector3 v = Vector3.zero; 
	
	float min = 0.20f;
	float max = 10f;
	float skip = 10.0f;
	float mod = 1;
	float time = 0.5f;
	
	// Use this for initialization
	void Start () 
	{
		cam = GetComponent<Camera>();
		if (player == null)
		{
			player = PlayerScript.FindPlayer();
		}
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if (player == null) 
		{
			player = PlayerScript.FindPlayer();
		} 
		
		Vector2 pos = cam.WorldToViewportPoint(player.transform.position);
		float x = pos.x-0.5f;
		float y = pos.y-0.5f;

		if (Mathf.Abs(x) < min) { x = 0; }
 		if (Mathf.Abs(y) < min) { y = 0; }
	
		mod += Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
		mod *= mod;
		mod *= mod;
		if (mod > max) {
			mod = max;
		}
	
		Vector3 delta = new Vector3(x, y, 0);
		Vector3 destination = transform.position + mod*skip*delta; 
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref v, time);	
		mod = 1;	
	}
}
