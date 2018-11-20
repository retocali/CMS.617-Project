using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour, ToolsInterface 
{
	public string sceneName = "l1";
	public GameObject load;
	
	public Direction outDirection = Direction.Left;

	private bool loaded = false;
	
	public float time = 2f;
	
	private void Update() {
		if (!loaded) { return; }
		time -= Time.deltaTime;
		if (time <= 0)
		{
			SceneManager.LoadSceneAsync(sceneName);
			time = 100f;
		}
	}
	public Vector3 Exit()
	{
		Vector3 origin = transform.position;
		Vector2 distance = GetComponent<BoxCollider2D>().size;
		switch (outDirection)
		{
			case Direction.Top:
				return origin + new Vector3(0, distance.y, 0);
			case Direction.Bottom:
				return origin + new Vector3(0, -distance.y, 0);
			case Direction.Left:
				return origin + new Vector3(-distance.x, 0, 0);
			case Direction.Right:
				return origin + new Vector3(distance.x, 0, 0);
			default:
				return origin;
		}
	}

	public void Interact(GameObject p)
	{
        if (loaded || Input.GetAxisRaw("Jump") == 0) { return; }
		load.SetActive(true);
		loaded = true;

	}

}
