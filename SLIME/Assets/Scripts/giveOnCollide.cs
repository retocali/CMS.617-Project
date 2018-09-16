using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveOnCollide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collider2D collider) {
		Destroy(gameObject);
	}
}
