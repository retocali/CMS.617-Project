using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ExplodingBossScript : MonoBehaviour {

	private ParticleSystem partSys;

	private bool end;

	public float time = 2;

	private bool loaded = false;


	// Use this for initialization
	void Start () {

		partSys = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

			time -= Time.deltaTime;
			if (time < 0 && !loaded)
			{
				loaded = true;
				SceneManager.LoadSceneAsync("EndGame");
			}

	}
	

	public void Explode(){
		gameObject.GetComponent<ParticleSystem>().Play();
		end = true;

	}

}
