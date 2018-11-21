using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour, ToolsInterface 
{
	public string sceneName = "l1";
	public GameObject load;
	
	private GameObject player;
	private PlayerScript ps;
	private Vector3 v = Vector3.zero; 

	public Direction outDirection = Direction.Left;

	private bool loaded = false;
	
	public float time = 2f;
	public float animateTime = 1f;
	
	private void Update() {
		transform.Rotate(new Vector3(0,0,-1));
		WithPlayer();
		if (!loaded) { return; }
		time -= Time.deltaTime;
		
		if (time <= 0)
		{
			Data.lastAttemptedScene = sceneName;
			SceneManager.LoadSceneAsync(sceneName);
			time = 2f;
		} 
		else if (time <= animateTime)
		{
			load.SetActive(true);
		}
	}

	private void WithPlayer()
	{ 
		if (ps == null) return;

		ps.DestroyCrumbs(0.25f);
		ps.MultiplyVelocity(0);
		player.transform.position = Vector3.SmoothDamp(player.transform.position, 
										 transform.position, ref v, animateTime/2);
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
        if (loaded || Input.GetAxisRaw("Jump") == 0) { return; }
		ps = p.GetComponent<PlayerScript>();
		ps.UnStun();
		if (ps.IsDead()) {
			ps = null;
			return;
		}
		player = p;
		loaded = true;

	}

}
