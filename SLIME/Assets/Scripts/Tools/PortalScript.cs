using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour, ToolsInterface 
{
	public string sceneName = "l1";
	public GameObject load;
	private bool loaded = false;
	public float time = 2f;
	
	private void Update() {
		if (!loaded) { return; }
		time -= Time.deltaTime;
		if (time <= 0)
		{
			SceneManager.LoadSceneAsync(sceneName);
		}
	}
	
	public void Interact(GameObject p)
	{
        if (loaded || Input.GetAxisRaw("Jump") == 0) { return; }
		load.SetActive(true);
		loaded = true;

	}
}
