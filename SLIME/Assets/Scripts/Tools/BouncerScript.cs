using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class BouncerScript : MonoBehaviour, ToolsInterface
{

    public float directionDeg = 0;
    public float magnitude = 1;

    private float disableTime = 0;

    private GameObject player;

    private Animator animator;

    
    public void Interact(GameObject player)
    {
        /**
         * Adds the bouncers velocity to the player on contact
         */
        
        if (disableTime <= 0)
        {
            Debug.Log("Bounce!");
            PlayerScript ps = player.GetComponent<PlayerScript>();
            if (ps != null)
            {
                Vector3 deltav = new Vector3(magnitude * Mathf.Cos(directionDeg * Mathf.PI / 180), magnitude * Mathf.Sin(directionDeg * Mathf.PI / 180));
                ps.AddVelocity(deltav);
                disableTime = 0.1f;
            }
            animator.SetTrigger("bounce");
        }
 

    }

    // Use this for initialization
    void Start () {
		Debug.Log(transform.position);

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator on " + gameObject.name + " does not exist");
            }
        } 
        
	}
	
	// Update is called once per frame
	void Update () {
        if(disableTime > 0)
        {
            disableTime -= Time.deltaTime;
        }
		
	}
}
