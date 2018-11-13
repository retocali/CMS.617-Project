using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadMaster : MonoBehaviour {

	private List<EnemyClass> enemies = new List<EnemyClass>();
	// TODO: For any other items that can also move 
	// this should be changed into a list of ToolsInterface
	// but since it's just bottles for our spec 
	private List<BottleScript> items = new List<BottleScript>();

	private static ReloadMaster instance;

	// Use this for initialization of singleton class
	void Awake() {
		instance = this;
	}

	// Reloads all objects in the scene, if an object 
	// has been destroyed since then, silently ignores it.
	public static void ReloadObjects()
	{
		if (instance == null) {
			Debug.LogWarning("Reload failed, init failed");
			return;
		}
		foreach (EnemyClass e in instance.enemies) {
			if (e != null && !e.spawned) {
				e.Respawn();
				e.gameObject.SetActive(true);
			} 
		}
		foreach (BottleScript i in instance.items) {
			i.Respawn();
		}
	}

	public static bool AddToMaster(EnemyClass e) 
	{
		if (instance == null) {
			Debug.LogWarning(e.name + " was not logged");
			return false;
		}
		instance.enemies.Add(e);
		return true;
	}

	public static bool AddToMaster(BottleScript b) 
	{
		if (instance == null) {
			Debug.LogWarning(b.name + " was not logged");
			return false;
		}
		instance.items.Add(b);
		return true;
	}

}
