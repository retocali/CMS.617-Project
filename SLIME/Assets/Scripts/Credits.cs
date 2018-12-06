using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public float speed=50;
    private bool loaded = false;
	// Use this for initialization
	void Start () {
        loaded = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        Debug.Log(Vector3.up);
        Debug.Log(Time.deltaTime);
        Debug.Log(speed);
        if (transform.position.y> 10 && !loaded)
        {
            speed=0;
            loaded = true;
            Data.started = true;
            SceneManager.LoadSceneAsync("hub-world");
        }
	}
}
