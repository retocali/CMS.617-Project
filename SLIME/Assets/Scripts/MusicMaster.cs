using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMaster : MonoBehaviour {

	public AudioClip urgency;
	public AudioClip original;
	
	private static AudioSource audsrc;
	private static MusicMaster instance;
	// Use this for initialization
	void Awake () {
		audsrc = GetComponent<AudioSource>();
		if (original == null)
			original = audsrc.clip;

		instance = this;
	}
	
	public static void SpawnUrgency()
	{
		Debug.Log("Urgency Music Starting");
		audsrc.clip = instance.urgency;
		audsrc.Play();
	}

	public static void DespawnUrgency()
	{
		Debug.Log("Original Music Starting");
		audsrc.clip = instance.original;
		audsrc.Play();
	}
}
