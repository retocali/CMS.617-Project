using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Controller2D))]
public class TeleporterScript : MonoBehaviour {

	public Vector3[] locations;
	public float[] times;
	private float gravity = -20f;
	private Vector3 velocity;
	private Controller2D c2d;

	private int i;
	private float currentTime;
	private float delayTime = 0.15f;
	private float delay = 0;
	private Transform portal;
	private bool functional = true;

	// Use this for initialization
	void Start () {
		i = 0;
		currentTime = 0;
		
		c2d = GetComponent<Controller2D>();
		if (locations.Length < 2 || times.Length < 2) {
			Debug.LogError("locations and times must have at least 2 items");
			functional = false;
			return;
		}
		if (locations.Length != times.Length) {
			Debug.LogError("locations and times must be the same length!");
			functional = false;
			return;
		}
		if (portal == null)
		{
			portal = transform.parent.GetChild(0);
			if (portal == null) {
				Debug.LogError("Teleporter structure is broken");
				functional = false;
				return;
			}
		}
		delay = delayTime*times[1];
	}
	
	// Update is called once per frame
	void Update () {
		if (!functional) { Debug.LogError(transform.name+" not functional"); return; }

		currentTime += Time.deltaTime;
		if (delay > 0)
		{
			delay -= Time.deltaTime;
			if (delay <= 0) {
				portal.position = locations[(i+1) % locations.Length];
			}
		}
		if (currentTime >= times[i]) {
			currentTime = 0;
			i = (i+1) % locations.Length;
			transform.position = locations[i];
			delay = delayTime*times[i];
		}
		velocity.y += Time.deltaTime*gravity;
		if (c2d.collision.above || c2d.collision.below) {
			velocity.y = 0;
		}
		if (c2d.collision.right || c2d.collision.left) {
			velocity.x = 0;
		}
		c2d.Move(velocity*Time.deltaTime);
	}
}

