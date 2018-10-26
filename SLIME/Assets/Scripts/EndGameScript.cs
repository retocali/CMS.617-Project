using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour {

	private bool touched = false;
	private float time = 2;
	private Transform player;
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (touched)
		{
			player.position = transform.position;
			Debug.Log(time);
			time -= Time.deltaTime;
			if (time < 0)
			{
				SceneManager.LoadScene("EndGame");
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider) 
    {
		Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Player") {
			touched = true;
			collider.gameObject.GetComponent<PlayerScript>().inactive = true;
			collider.gameObject.GetComponent<PlayerScript>().MultiplyVelocity(0);
			player = collider.gameObject.transform;
		}
    }
}
