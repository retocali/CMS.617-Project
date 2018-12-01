using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour {

	public GameObject wind;
	public float minSize = 0.5f;
	public float maxSize = 1.1f;
	

	private float timeLeft = 0;
	public float gap = 2f;
	public const int colorChanges = 100;
	private Color startColor;
	private Color endColor;

	float time;

	// Use this for initialization
	void Start () {
		timeLeft = gap;
		 Animator anim = wind.GetComponent<Animator>();
 		RuntimeAnimatorController ac = anim.runtimeAnimatorController;    //Get Animator controller
		for(int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
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
			timeLeft = gap;
		}
	}

	 public void SpawnRandom()
     {
 
        Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0,Screen.width), Random.Range(0,Screen.height), Camera.main.farClipPlane/2));
        GameObject w = Instantiate(wind,screenPosition,Quaternion.identity);
		w.transform.localScale = new Vector3(1,1,1)*Random.Range(minSize, maxSize);
		startColor = Random.ColorHSV(0f,1f, 0.99f, 1f, 0.99f, 1f);
		float h,s,v;
		Color.RGBToHSV(startColor, out h, out s, out v);
		
		endColor =   Random.ColorHSV(h-0.01f, h+0.01f,0.2f,1f,0.2f,1f);
		Color.RGBToHSV(endColor, out h, out s, out v);
		endColor.a = 0.25f;
		StartCoroutine(DestroyWind(w));
     }

	 IEnumerator DestroyWind(GameObject w) {
		
		for (int i = 0; i < colorChanges; i++) {
			w.GetComponent<SpriteRenderer>().color = Color.LerpUnclamped(startColor, endColor, i*time/colorChanges);
			// Debug.Log(w.GetComponent<SpriteRenderer>().color);
			yield return new WaitForSeconds(time/colorChanges);
		}
		Destroy(w);

	 }
}
