using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldMaster : MonoBehaviour {

	public PortalScript[] portals;
	public GameObject player;
	private float magnitude = 4f;

	// Use this for initialization
	void Start () {
		if (Data.lastAttemptedScene != "") {
			foreach (PortalScript p in portals) {
				if (p.sceneName == Data.lastAttemptedScene) {
					player.transform.position = p.Exit();
					Vector3 velocity = p.Exit()-p.gameObject.transform.position;
					if(p.sceneName == "lfinal") {
						player.GetComponent<PlayerScript>().AddVelocity(velocity*20);
					}
					else {
						player.GetComponent<PlayerScript>().AddVelocity(velocity*magnitude);
					}
					return;
				}
			}
		}
	}
}
