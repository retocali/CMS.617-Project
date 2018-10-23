using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterMasterScript : MonoBehaviour {


	public GameObject[] splitters;
	// Use this for initialization
	void Start () {
		
	}

	public void resetSplitters() {
		for (int i = 0; i < splitters.Length; i++) {
			splitters[i].GetComponent<SplitterScript>().resetSplitter();
		}
	}
	
	// Update is called once per frame
	void Update () {

		
	}
}
