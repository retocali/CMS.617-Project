using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject player;
	private Camera cam;
	private Vector3 v = Vector3.zero; 
	private Vector3 destination = Vector3.zero;
	float gap = 0.2f;
	float skip = 15.0f;
	float time = 0.3f;
	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 pos = cam.WorldToViewportPoint(player.transform.position);
		float x = pos.x-0.5f;
		float y = pos.y-0.5f;
		if (Mathf.Abs(x) < gap)
			x = 0;
		if (Mathf.Abs(y) < gap)
			y = 0;
		Vector3 delta = new Vector3(x, y, 0);
		Vector3 destination = transform.position + skip*delta; 
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref v, time);	
	}
}
