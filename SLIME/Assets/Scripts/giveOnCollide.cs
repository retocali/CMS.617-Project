using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giveOnCollide : MonoBehaviour
{

    private Camera cam;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 pos = cam.WorldToViewportPoint(transform.position);
	   // if (pos.y < 0 || pos.x < 0 
		//  || pos.y > 1 || pos.x > 1)
        // {
		// 	Destroy(gameObject);
		// 	Destroy(this);
        // }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        Destroy(gameObject);
    }
}
