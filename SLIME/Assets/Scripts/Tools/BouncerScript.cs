using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BouncerScript : MonoBehaviour, ToolsInterface
{

    public float directionDeg = 0;
    public float magnitude = 1;

    private GameObject player;

    
    public void Interact(GameObject player)
    {
        /**
         * Adds the bouncers velocity to the player on contact
         */
        Debug.Log("Bounce!");
        PlayerScript ps = player.GetComponent<PlayerScript>();
        if(ps != null)
        {
            Vector3 deltav = new Vector3(magnitude * Mathf.Cos(directionDeg * Mathf.PI / 180), magnitude * Mathf.Sin(directionDeg * Mathf.PI / 180));
            ps.AddVelocity(deltav);
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
