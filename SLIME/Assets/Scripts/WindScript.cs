using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour {

	public GameObject wind;

	private float timeLeft = 5;

	float time;

	// Use this for initialization
	void Start () {
		 Animator anim = wind.GetComponent<Animator>();
 		RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
     for(int i = 0; i<ac.animationClips.Length; i++)                 //For all animations
     {
         if(ac.animationClips[i].name == "Wind Animation")        //If it has the same name as your clip
         {
             time = ac.animationClips[i].length;
			 break;
         }
     }
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if ( timeLeft < 0 )
		{
			SpawnRandom();
			timeLeft = 5;
		}
	}

	 public void SpawnRandom()
     {
 
         Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0,Screen.width), Random.Range(0,Screen.height), Camera.main.farClipPlane/2));
         GameObject w = Instantiate(wind,screenPosition,Quaternion.identity);
		 StartCoroutine(DestroyWind(w));
     }

	 IEnumerator DestroyWind(GameObject w) {
		 yield return new WaitForSeconds(time);
		 Destroy(w);

	 }
}
