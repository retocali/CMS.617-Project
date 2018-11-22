using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : EnemyClass 
{

	public Vector3[] locations;
	public float[] times;
	
	private int i;
	private float currentTime;
	private float delayTime = 0.85f;
	private float delay = 0;
	private Transform portal;

    private Animator animor;

	// Use this for initialization
	new void Start () {
        animor = GetComponent<Animator>();
		i = 0;
		currentTime = 0;
		if (locations.Length < 2 || times.Length < 2) {
			Warn("locations and times must have at least 2 items");
			return;
		}
		if (locations.Length != times.Length) {
			Warn("locations and times must be the same length!");
			return;
		}
		if (portal == null)
		{
			portal = transform.parent.GetChild(0);
			if (portal == null) {
				Warn("Teleporter structure is broken");
				return;
			}
		}
		delay = delayTime*times[1];
		gravity = -20f;
		base.Start();
	}
	public override void Respawn()
	{
		i = 0;
		currentTime = 0;
		delay = delayTime*times[1];
		base.Respawn();
	}

	// Update is called once per frame
	new void Update () {
		if (!functional) { Warn(transform.name+" not functional"); return; }

		currentTime += Time.deltaTime;
		if (delay > 0)
		{
			delay -= Time.deltaTime;
			if (delay <= 0) {
				portal.position = locations[(i+1) % locations.Length];
                animor.SetTrigger("teleport");
			}
		}
		if (currentTime >= times[i]) {
			currentTime = 0;
			i = (i+1) % locations.Length;
			transform.position = locations[i];
			delay = delayTime*times[i];
            animor.SetTrigger("apear");
        }
		ApplyGravity(ref velocity, Time.deltaTime);
		base.Update();
	}
}

