using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

	public Color initial = Color.white;
	public Color hover = Color.red;
	private Color target;
	private float speed = 15f;
	
	public bool start = true;
	public string sceneName = "hub-world";
	public GameObject load;
	private bool loaded = false;
	private float time = 5f;
	
	private TextMesh text;
	private TextMesh title;
	private SpriteRenderer bg1;
	private SpriteRenderer bg2;

	private bool pressed;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMesh>();
		pressed = false;
		target = initial;
	
		if (start)
		{
			title = transform.GetChild(0).GetComponent<TextMesh>();
			bg1 = transform.GetChild(1).GetComponent<SpriteRenderer>();
			bg2 = transform.GetChild(2).GetComponent<SpriteRenderer>();
			StartCoroutine(FadeIn());
		}
	}
	
	IEnumerator FadeIn() 
	{
		
		for (float i = 0; i <= 1; i += Time.deltaTime/2f)
		{
			// set color with i as alpha
			bg1.material.color = new Color(1, 1, 1, i);
			bg2.material.color = new Color(0, 0, 0, i);
			text.color = new Color(1, 1, 1, i);
			title.color = new Color(0.508f, 1f, 0.508f, i);
			yield return null;
		}
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
			Data.started = true;
			Destroy(gameObject);
		} else {
			load.SetActive(true);
			loaded = true;
		}
	}
	
}
