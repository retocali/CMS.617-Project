using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubWorldMaster : MonoBehaviour {

	public PortalScript[] portals;
	public GameObject player;

	// Use this for initialization
	void Start () {
		Debug.Log(Data.lastCompleted());
		if (Data.lastCompleted() != "") {
			foreach (PortalScript p in portals) {
				Debug.Log(p.sceneName);
				if (p.sceneName == Data.lastCompleted()) {
					player.transform.position = p.Exit();
					Debug.Log("Teleporting");
					return;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
