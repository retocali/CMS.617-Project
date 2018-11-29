using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject player;

	private Camera cam;
	private Vector3 v = Vector3.zero; 
	
	float min = 0.15f;
	float max = 3f;
	float skip = 25.0f;
	float mod = 1;
	float time = 0.75f;
	
	private bool focus = false;
	private bool timedFocus = false;
	private float focusTime = 0;
	private Vector3 focusLoc;
	// Use this for initialization
	void Start () 
	{
		cam = GetComponent<Camera>();
	}
	// Update is called once per frame
	void LateUpdate () 
	{
		if (!Data.started) {
			return;
		}

		if (player == null) 
		{
			player = PlayerScript.FindPlayer();
			transform.position = new Vector3(player.transform.position.x,
											 player.transform.position.y,
													transform.position.z);
		} 
		
		if (focus)
		{
			transform.position = focusLoc;
			return;
		}

		if (timedFocus)
		{
			focusTime -= Time.deltaTime;
			if (focusTime < 0)
			{
				timedFocus = false;
			}
			else 
			{
				transform.position = focusLoc;
				return;
			}
		}
		

		Vector2 pos = cam.WorldToViewportPoint(player.transform.position);
		float x = pos.x-0.5f;
		float y = pos.y-0.5f;

		if (Mathf.Abs(x) < min) { x = 0; }
 		if (Mathf.Abs(y) < min) { y = 0; }
	
		AdjustModifier(x, y);
	
		Vector3 delta = new Vector3(x, y, 0);
		Vector3 destination = transform.position + skip*delta; 
		// ClampToPlayer(delta, ref destination);
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref v, time/mod);	
		
		
		mod = 1;	
	}
	private void AdjustModifier(float x, float y)
	{
		mod += Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
		mod *= mod;
		if (mod > max) {
			mod = max;
		}
	}

	public void CameraFocusTimed(Vector2 loc, float time)
	{
		focusLoc = new Vector3(loc.x, loc.y, transform.position.z);
		focusTime = time;
		timedFocus = true;
	}

	public void CameraFocusOn(Vector2 loc)
	{
		focusLoc = new Vector3(loc.x, loc.y, transform.position.z);
		focus = true;
	}
	
	public void CameraFocusOff()
	{
		focusTime = 0;
		focus = false;
		timedFocus = false;
		
	}
}
