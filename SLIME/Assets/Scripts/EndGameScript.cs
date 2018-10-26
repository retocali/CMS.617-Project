using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Here");	
	}
	// void OnCollisionEnter2D(Collision2D collider) 
    // {
	// 	Debug.Log(collider.gameObject.tag);
    //     if (collider.gameObject.tag == "Player") {
	// 		SceneManager.LoadScene("EndGame");
	// 	}
    // }
	void OnTriggerEnter2D(Collider2D collider) 
    {
		Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Player") {
			SceneManager.LoadScene("EndGame");
		}
    }
}
