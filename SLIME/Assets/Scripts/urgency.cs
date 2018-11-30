using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class urgency : MonoBehaviour {

    public GameObject player;
    public ArrayList list;  
    private SpriteRenderer sprend;
    ///This item will be at the player's position in time frames
    public float speed=10;
    public float time=100;
    public bool constSpeed=false;
    public int samplingFreq=100;
    private int counter;
    private Rigidbody2D rb;
    private Vector2 startingPos;
    public enum ResetType{
        //the block doesn't move
        STAY,
        //moves back to starting position
        BACK_TO_ORIGIN,
        //where it was at last checkpoint
        //CHECKPOINT_SNAPSHOT,
    }
    public ResetType onReset=ResetType.STAY;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sprend = GetComponent<SpriteRenderer>();
        startingPos=new Vector2(rb.position.x,rb.position.y);
        counter=samplingFreq;
    }
    // Update is called once per frame
    void Update () {
        //refactor into seperate method
        if (player == null || player.GetComponent<PlayerScript>().IsDead()) 
        {
            player = PlayerScript.FindPlayer();
            if(onReset==ResetType.BACK_TO_ORIGIN){
                Debug.LogWarning(rb.position);
                rb.position=new Vector2(startingPos.x,startingPos.y);
                Debug.LogWarning(rb.position);
            }

            Debug.LogWarning("Reseting position");
            return;
        }
        if(!constSpeed && counter--==0) {
            speed=Vector2.Distance(rb.position,player.transform.position)/time;
            counter=samplingFreq;
        }
        rb.velocity = (player.transform.position - transform.position).normalized * speed;

        // Update sprite flippiness
        if(rb.position.x  - player.transform.position.x > 0)
        {
            sprend.flipX = true;
        }
        else
        {
            sprend.flipX = false;
        }
    }

    public void changeSpeed(int s){
        speed = s;
    }
}
