using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

	private Color initial = Color.white;
	private Color hover = Color.red;
	private Color target;
	private float speed = 10;
	private GameObject player;

	private TextMesh text;
	private bool pressed;

	// Use this for initialization
	void Start () {
		text = GetComponent<TextMesh>();
		pressed = false;
		target = initial;
		player = PlayerScript.FindPlayer();
		player.GetComponent<PlayerScript>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!pressed)
			Input.ResetInputAxes();
		text.color = Color.Lerp(text.color, target, Time.deltaTime*speed);
	}
	private void OnMouseOver() {
		target = hover;
	}
	private void OnMouseExit() {
		target = initial;
	}
	private void OnMouseUpAsButton() {
		pressed = true;
		player.GetComponent<PlayerScript>().enabled = true;
		Destroy(gameObject);
	}
	
}
