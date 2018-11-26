using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldMaster : MonoBehaviour {

	public PortalScript[] portals;
	public GameObject player;

	// Use this for initialization
	void Start () {
		if (Data.lastAttemptedScene != "") {
			foreach (PortalScript p in portals) {
				if (p.sceneName == Data.lastAttemptedScene) {
					player.transform.position = p.Exit();
					return;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
