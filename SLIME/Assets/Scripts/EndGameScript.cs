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
			player.GetComponent<PlayerScript>().MultiplyVelocity(0);

			time -= Time.deltaTime;
			if (time < 0)
			{
				Data.markLevelCompleted(SceneManager.GetActiveScene().name);
				SceneManager.LoadSceneAsync("hub-world");
				time = 2f;
			}
			transform.localScale *= 1.05f;
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player") {
			touched = true;
			collider.enabled = false;
			player = collider.gameObject.transform;
		}
    }
}
