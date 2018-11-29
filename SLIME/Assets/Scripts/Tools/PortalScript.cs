using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour, ToolsInterface 
{
	public string sceneName = "l1";
	public GameObject load;
	
	private GameObject player;
	private PlayerScript ps;
	private float r = 0;

	public Direction outDirection = Direction.Left;

	private bool loaded = false;
	
	private const float time = 10f;
	private const int rev = 4;
	private float t = 0;
	private float animateTime = 5f;
	private float initAngle = 0f;	
	private void Start() 
	{
		t = time;
		r =	GetComponent<CircleCollider2D>().radius*transform.localScale.x;
	}

	private void Update() {
		transform.Rotate(new Vector3(0,0,-1));
		if (!loaded) { return; }
		WithPlayer();
		t -= Time.deltaTime;
		
		if (t <= 0)
		{
			Data.lastAttemptedScene = sceneName;
			SceneManager.LoadSceneAsync(sceneName);
			t = time;
		} 
		else if (t <= animateTime+0.25)
		{
			load.GetComponentInChildren<Text>().text = "Loading Level: " + sceneName.Substring(1) + "..";
			load.SetActive(true);
		}
		
	}

	private void WithPlayer()
	{ 
		if (ps == null) return;

		ps.DestroyCrumbs(0.25f);
		ps.MultiplyVelocity(0);
		
		float t_norm = (t-animateTime)/(time-animateTime);
		float angle = (2*Mathf.PI*t_norm*rev)+initAngle;

		Vector3 new_position = transform.position + (new Vector3(Mathf.Cos(angle), 
																 Mathf.Sin(angle),
																 0)*r*t_norm*t_norm);
		player.transform.position = new_position;
	}

	public Vector3 Exit()
	{
		Vector3 origin = transform.position;
		float distance = GetComponent<CircleCollider2D>().radius;
		switch (outDirection)
		{
			case Direction.Top:
				return origin + new Vector3(0, distance, 0);
			case Direction.Bottom:
				return origin + new Vector3(0, -distance, 0);
			case Direction.Left:
				return origin + new Vector3(-distance, 0, 0);
			case Direction.Right:
				return origin + new Vector3(distance, 0, 0);
			default:
				return origin;
		}

	}

	public void Interact(GameObject p)
	{
        if (loaded) { return; }
		ps = p.GetComponent<PlayerScript>();
		ps.UnStun();
		if (ps.IsDead()) {
			ps = null;
			return;
		}
		player = p;
		Vector3 delta = player.transform.position-transform.position;
		initAngle = Mathf.Atan2(delta.y, delta.x);
		loaded = true;

	}

}
