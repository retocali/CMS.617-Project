using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class FlierScript : MonoBehaviour {

	public Vector2[] directions;
	public float[] times;
	public bool loop = false;

	private int i;
	private float currentTime;
	private Vector3 velocity;
	private Controller2D c2d;
	private bool functional = true;
	private float speed = 10f;

	// Use this for initialization
	void Start () {
		i = 0;
		currentTime = 0;
		velocity = Vector3.zero;
		c2d = GetComponent<Controller2D>();
		if (directions.Length == 0 || times.Length == 0) {
			Debug.LogError("Directions and times must not be empty");
			functional = false;
		}
		if (directions.Length != times.Length) {
			Debug.LogError("Directions and times must be the same length!");
			functional = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!functional) { Debug.LogError(transform.name+" not functional"); return; }

		currentTime += Time.deltaTime;
		if (currentTime >= times[i]) {
			currentTime = 0;
			i = (i+1) % directions.Length;

		}
		
		velocity.x = speed * directions[i].x;
		velocity.y = speed * directions[i].y;
		if (c2d.collision.above || c2d.collision.below) {
			velocity.y = 0;
		}
		if (c2d.collision.right || c2d.collision.left) {
			velocity.x = 0;
		}

		c2d.Move(velocity*Time.deltaTime);
	}
}
