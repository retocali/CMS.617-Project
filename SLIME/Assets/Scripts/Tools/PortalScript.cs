using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScript : MonoBehaviour, ToolsInterface 
{
	public string sceneName = "l1";
	private bool loaded = false;
	public void Interact(GameObject p)
	{
        if (loaded || Input.GetAxisRaw("Jump") == 0) { return; }
		loaded = true;
		transform.localScale *= 100;
		SceneManager.LoadSceneAsync(sceneName);

	}
}
