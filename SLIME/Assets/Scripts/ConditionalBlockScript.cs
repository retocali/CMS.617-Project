using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalBlockScript : MonoBehaviour {

	public int numLevels = 0;
	// Use this for initialization
	void Start () {
		
		if (numLevels <= Data.getLevelsCompleted())
			Destroy(gameObject);	
		Debug.Log(Data.getLevelsCompleted());
	}
	
}
