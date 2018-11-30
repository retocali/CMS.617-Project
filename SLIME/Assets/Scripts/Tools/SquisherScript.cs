using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SquisherScript : MonoBehaviour, ToolsInterface {

	public GameObject splitter;

	int numSlimes = 0;

	private float y; 
 	private float x;
	private GameObject[] players;


	private GameObject mainPlayer = null;
	private GameObject player2 = null;
	public int release = 54;
	private int timeToRelease;

 	// Use this for initialization
	void Start () {
		y = gameObject.transform.position.y;
		x = gameObject.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (!splitter.GetComponent<SplitterScript>().isSplit()) {
			resetSquisher();
		}
		else {
			if (player2 != null) {
				player2.GetComponent<PlayerScript>().MultiplyVelocity(0);
			}
			if (mainPlayer != null) {
				mainPlayer.GetComponent<PlayerScript>().MultiplyVelocity(0);
			}
			if (numSlimes == 2) {
				
				WithPlayer();
			}
		}

		
	}

	public void WithPlayer() {
		Debug.Log(timeToRelease);
		if (timeToRelease > 0) { timeToRelease -= 1; }
		if (timeToRelease <= 0) {
			Release();

		}
	}

	public void Release(){
		Destroy(player2);
		mainPlayer.transform.position += new Vector3(0, -1.2f, 0);
		mainPlayer.GetComponent<PlayerScript>().AddVelocity(new Vector3(0, 12, 0));
		splitter.GetComponent<SplitterScript>().resetSplitter();
		resetSquisher();
	}

	public void Interact(GameObject p)
	{
		if (splitter.GetComponent<SplitterScript>().isSplit() && !players.Contains(p)) {
			timeToRelease = release;
			if (!p.GetComponent<PlayerScript>().IsMain()) {
				player2 = p;
				players[0] = p;
			}
			else {
				mainPlayer = p;
				players[1] = p;
				
				StartCoroutine(switchPlayers());
			}
			p.transform.position = new Vector3(x, y, 0);
			p.GetComponent<PlayerScript>().MultiplyVelocity(0);
			numSlimes++;
			
		}



	}
	IEnumerator switchPlayers()
	{
		yield return new WaitForSeconds(0.5f);
		Camera.main.gameObject.GetComponent<CameraScript>().SwitchPlayers(splitter.GetComponent<SplitterScript>().player2);
	}
	private void resetSquisher() {
		player2 = null;
		mainPlayer = null;
		numSlimes = 0;
		timeToRelease = release;
		players = new GameObject[2];
	}
}
