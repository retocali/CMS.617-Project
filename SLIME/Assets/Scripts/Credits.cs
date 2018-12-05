﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public float speed=50;
    private bool loaded = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        Debug.Log(transform.position.y);
        if (transform.position.y> 13 && !loaded)
        {
            speed=0;
            loaded = true;
            Data.started = true;
            SceneManager.LoadSceneAsync("hub-world");
        }
	}

    private void OnBecameInvisible() {
        Debug.Log("YOO");
    }
}
