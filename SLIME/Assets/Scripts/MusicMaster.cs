using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMaster : MonoBehaviour {

	public AudioClip urgency;
	public AudioClip original;
	public AudioClip roar;
	public float gapTime = 0.5f;
	
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
		instance.PlayMusicWrapper(instance.urgency);
	}

	public static void DespawnUrgency()
	{
		instance.PlayMusicWrapper(instance.original);
	}
	private void PlayMusicWrapper(AudioClip clip)
	{
		StartCoroutine(musicTransition(clip));
	}

	private IEnumerator musicTransition(AudioClip clip)
	{
		audsrc.Stop();
		yield return new WaitForSeconds(gapTime);
		audsrc.clip = roar;
		audsrc.Play();
        yield return new WaitForSeconds(roar.length+gapTime);
		audsrc.clip = clip;
		audsrc.Play();
	}
}
