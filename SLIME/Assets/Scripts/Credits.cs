using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    public float speed=50;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        if (gameObject.transform.position.y - ((RectTransform)gameObject.transform).rect.height> 0)
        {
            speed=0;
            SceneManager.LoadSceneAsync("hub-world");
        }
	}
}
