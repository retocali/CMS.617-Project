using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

	public Color initial = Color.white;
	public Color hover = Color.red;
	private Color target;
	private float speed = 5f;
	
	public bool start = true;
	public string sceneName = "hub-world";
	public GameObject load;
	private bool loaded = false;
	private float time = 5f;
	
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
		if (Input.GetAxisRaw("Jump") != 0) {
			OnMouseUpAsButton();
		}
		text.color = Color.Lerp(text.color, target, Time.deltaTime*speed);
		if (loaded) {
			time -= Time.deltaTime;
			if (time <= 0)
			{
				SceneManager.LoadSceneAsync(sceneName);
				time = 100f;
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
