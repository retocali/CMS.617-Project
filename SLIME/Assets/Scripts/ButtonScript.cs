using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

	private Color initial = Color.white;
	private Color hover = Color.red;
	private Color target;
	private float speed = 10;
	
	public bool start = true;
	public string sceneName = "hub-world";
	public GameObject load;
	private bool loaded = false;
	public float time = 2f;
	
	private TextMesh text;
	private bool pressed;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMesh>();
		pressed = false;
		target = initial;
	}
	
	// Update is called once per frame
	void Update () {
		text.color = Color.Lerp(text.color, target, Time.deltaTime*speed);
		if (loaded) {
			time -= Time.deltaTime;
			if (time <= 0)
			{
				SceneManager.LoadSceneAsync(sceneName);
			}
		}
	}
	private void OnMouseOver() {
		target = hover;
	}
	private void OnMouseExit() {
		target = initial;
	}
	private void OnMouseUpAsButton() {
		pressed = true;
		if (start) {
			Destroy(gameObject);
		} else {
			load.SetActive(true);
			loaded = true;
		}
	}
	
}
