using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]
public class PlayerScript : MonoBehaviour 
{
	public float jumpTime = 0.5f;
	public float jumpHeight = 10f;
	public float acceleration = 10f;
	public bool mainPlayer = true;

	private float minWallJumpSpeed = 15f;
	private float maxWallJumpSpeed = 25f;
	private float wallJumpAngle = Mathf.Deg2Rad*35f;
	private float wallJumpModifier = 1.1f;

	private float jumpVelocity;
	private float extendJumpModifier = 1.5f;
	
	private float gravity;
	private float increaseGravityModifier = 1.5f;
	
	private float maxSpeedY = 50f;
	private float maxSpeedX = 50f;
	private float maxSpeedThresholdX = 10f;
	private float minSpeedThresholdX = 1f;

	private Vector3 prevVelocity;
	private Vector3 velocity = Vector3.zero;
	private Vector3 addedVelocity = Vector3.zero;
	private float velocityModifier = 1;
	
	private Controller2D c2d;
	private SpriteRenderer sprRend;
	private AudioSource audSrc;
	private ParticleSystem partSys;
    private Animator animor;
	public GameObject pausedScreen;

	private bool dead = false;
	public bool inactive = false;
	private bool stunned = false;
	private float stunCounter = 0;
	private float stunCountModifier = 0.15f;
	private float stunDropModifier = 3.5f;
	private Color defaultColor;

    // Bounce deceleration delay
    private float bounceDelayTime = 0;
    private float maxBounceDelayTime = 5;

	public GameObject crumbPrefab;
	private float crumbSpace = 2.0f;
	private int crumbNum = 50;
	private bool trailing = true;
	private GameObject[] crumbs;
	private int crumbIndex = 0;
	private float crumbGap = 0f;

	public AudioClip deathSound;
	public AudioClip bounceSound;
	public AudioClip wallJumpSound;
	private int playing = 3;

    private bool paused=false;
    private bool muted=false;
    private string homeScreen="hub-world";
	// Use this for initialization
	void Start () 
	{
	
		float t = jumpTime/2;
		jumpVelocity = 2.0f*jumpHeight/t;
		gravity = -jumpVelocity/t;
		
		sprRend = GetComponentInChildren<SpriteRenderer>();
		c2d = GetComponent<Controller2D>();
		audSrc = GetComponent<AudioSource>();
		partSys = GetComponent<ParticleSystem>();
        animor = GetComponentInChildren<Animator>();

		defaultColor = sprRend.material.color;
		crumbs = new GameObject[crumbNum];
	}

	void OnTriggerEnter2D(Collider2D collider) { Touch(collider); }
	void OnTriggerStay2D(Collider2D collider)  { Touch(collider); }
	void OnTriggerExit2D(Collider2D collider)  { Touch(collider); }
	void Touch(Collider2D collider) {
		if(collider.tag == "hazard") {
			KillPlayer();
		}
	}
	
//
//	 Public Methods
//
///////////////////////////////////////
	
	/**
		Static method to hopefully find the player
		uses common names for the player to find it 
	 */
	public static GameObject FindPlayer() 
	{
		GameObject player = GameObject.Find("Player");
		if (player == null) 
		{
			player = GameObject.Find("Player(Clone)");
			if (player == null) 
			{
				Debug.LogError("Error: Could not find player prefab? Name might have changed");
				return null;
			}
		}
		return player;
	}

	/**
		"Spawns" the player by reseting it
		 and moving to the given location
	 */
	public void SpawnPlayer(Vector3 origin)
	{
		StartCoroutine(spawningPlayer(origin));
	}
	private IEnumerator spawningPlayer(Vector3 origin)
	{
		yield return new WaitForSeconds(0.25f);
		partSys.Stop();
		sprRend.material.color = defaultColor;
		dead = false;
		prevVelocity = Vector3.zero;
		velocity = Vector3.zero;

		this.transform.position = origin;
	}

	/**
		"Kills" the player by disabling input
		and tinting it.
	 */
	public void KillPlayer()
	{
		if (!dead) { 
			audSrc.PlayOneShot(deathSound);
			partSys.Clear();
			partSys.Play();
		}
		DestroyCrumbs(1.5f);
		dead = true;
		velocity = Vector3.zero;
	}
	
	/**
		Returns whether the player is alive or not
	 */
	public bool IsDead() 
	{
		return dead; 
	}

	/**
		Returns whether the player is the main slime when split (meant to keep references
		for checkpoint master)
	*/
	 	public bool IsMain() 
	{
		return mainPlayer; 
	}

	/**
		Returns whether the player is the main slime when split (meant to keep references
		for checkpoint master)
	*/
	public void duplicateSlime() 
	{
		mainPlayer = false;
	}
	
	/**
		Destroys all the crumbs the player produces
	 */
	public void DestroyCrumbs(float t) {
		for (int i = 0; i < crumbNum; i++) {
			Destroy(crumbs[i], t);
		}
	}
	/**
		"Stuns" the player which causes them to be unable to 
		move in any direction 
		@param count: seconds for how long the player will be
		stunned
	 */
	public void StunPlayer(float count) 
	{
		sprRend.material.color = new Color(0.5f, 0.75f, 0.5f);
		velocity = Vector3.zero;
		prevVelocity = Vector3.zero;
		stunned = true;
		trailing = false;
		DestroyCrumbs(0);
		stunCounter = count;
	}
	public void UnStun() 
	{
		trailing = true;
		stunCounter = 0; 
		stunned = false;
		sprRend.material.color = defaultColor;
	}

	/**
		Public getter for velocity
	 */
	public Vector3 Velocity()
	{
		return new Vector3(velocity.x, velocity.y, velocity.z);
	}

	/**
		A method used for adding velocity to the player
		negative numbers will decrease speed. Accumulates
	 */
	public void AddVelocity(Vector3 v)
	{
		addedVelocity += v;
	}

	/**
		Used to multiply the velocity of the player
		in a safe manner, overwrites previous value;
	 */
	public void MultiplyVelocity(float f)
	{
		velocityModifier = f;
	}
//
//	 Private Methods
//
///////////////////////////////////////
	private void Trail() {
		if (!trailing) {return;}
		crumbGap += crumbNum*Time.deltaTime;
		if (crumbGap < crumbSpace) {
			return;
		}
		crumbGap = 0;
		Destroy(crumbs[crumbIndex]);
		crumbs[crumbIndex] = Instantiate(crumbPrefab,
								new Vector3(transform.position.x,transform.position.y,-6),
								transform.rotation) as GameObject;
		crumbIndex = (crumbIndex + 1) % crumbNum;
	}
	/**
		Complement to above, is used to eventually
		"unstun" the character or allow them to move again.
	 */
	private void CheckUnStun() 
	{
		stunCounter -= Time.deltaTime;
		velocity = Vector3.zero;
		prevVelocity = Vector3.zero;
		if (stunCounter <= 0)
		{ 
			UnStun();
		}
	}

	/**
		Controls the calculation of velocities based on the 
		bounces the player must make. This calculates wall jumps,
		ground pounds, high jumps, and high-velocity splats
		
		@param velocity: current player velocity to be adjusted
		@param input: vector3 contained player inputs
	 */
	private void Bounce(ref Vector3 velocity, Vector3 input)
	{
		if (c2d.collision.below || c2d.collision.above) 
		{
			velocity.y = c2d.collision.below? jumpVelocity:-jumpVelocity;
			PlayAudio(bounceSound);
			float maxDrop = -jumpVelocity*stunDropModifier;
			if (prevVelocity.y < maxDrop) 
			{
				StunPlayer(Mathf.Abs(prevVelocity.y-maxDrop)*stunCountModifier);
			}
			else if (input.y ==  1) { velocity.y *= extendJumpModifier; }
			else if (input.y == -1) { velocity.y *= increaseGravityModifier; }
		}
		if (c2d.collision.right || c2d.collision.left) 
		{
			if (input.z != 0) 
			{
				float speed = Mathf.Sqrt(velocity.x*velocity.x+velocity.y*velocity.y);
				PlayAudio(wallJumpSound);
				if (speed > maxWallJumpSpeed) {
					velocity.y = wallJumpModifier*maxWallJumpSpeed*Mathf.Sin(wallJumpAngle);
					velocity.x = wallJumpModifier*maxWallJumpSpeed*Mathf.Cos(wallJumpAngle)*-Mathf.Sign(velocity.x);
				} else if (speed > minWallJumpSpeed) 
				{
					
					velocity.y = wallJumpModifier*speed*Mathf.Sin(wallJumpAngle);
					velocity.x = wallJumpModifier*speed*Mathf.Cos(wallJumpAngle)*-Mathf.Sign(velocity.x);
				} 
				else 
				{
					velocity.y = wallJumpModifier*minWallJumpSpeed*Mathf.Sin(wallJumpAngle);
					velocity.x = wallJumpModifier*minWallJumpSpeed*Mathf.Cos(wallJumpAngle)*-Mathf.Sign(velocity.x);
				}
			} 
			else 
			{
				velocity.x *= -0.5f;
			}
		} 
	}

	/**
		Controls the calculation of velocities in x direction
		and clamping it.
		
		@param velocity: current player velocity to be adjusted
		@param input: vector3 contained player inputs
	 */
	private void Move(ref Vector3 velocity, Vector3 input)
	{
		
		if (Mathf.Abs(velocity.x) < maxSpeedThresholdX)
		{
			velocity.x += input.x * acceleration * Time.deltaTime;
		} else if (Mathf.Sign(input.x) != Mathf.Sign(velocity.x)) {
			velocity.x += input.x * acceleration * Time.deltaTime;
		}

        if (velocity.x > 0)
        {
            sprRend.flipX = true;
        } else if (velocity.x < 0)
        {
            sprRend.flipX = false;
        }
		
		if (input.x == 0) 
		{ 
			if (c2d.collision.below) 
			{ 
				// Stops when the player hits the ground
				velocity.x = 0; 
			}
			else 
			{	
				// Slowly decrease until a threshhold to stop
				float sign = Mathf.Sign(velocity.x);
				velocity.x -= sign * acceleration * Time.deltaTime;
				if (Mathf.Abs(velocity.x) < minSpeedThresholdX) 
				{
					velocity.x = 0;
				}
			}
		}
	} 

	/**
		Applies gravity in the y direction and clamps y velocity

		@param velocity: current player velocity to be adjusted
		@param input: vector3 contained player inputs
	 */
	private void ApplyGravity(ref Vector3 velocity, Vector3 input)
	{
		float gravityDelta = gravity * Time.deltaTime;
		if (input.y == -1) // Ground Pound
		{ 
			gravityDelta *= increaseGravityModifier*increaseGravityModifier; 
		}
		velocity.y += gravityDelta;	
		// Just in case 
		if (transform.position.y > 1<<10 || transform.position.y < -(1<<10)) {
			Debug.LogWarning("Player escaped bounds, edit bounds if this was intentional");
			KillPlayer();
		}
	}

	private void ClampSpeeds(ref Vector3 velocity)
	{
		velocity.x = Mathf.Max(Mathf.Min(maxSpeedX, velocity.x), -maxSpeedX);
		velocity.y = Mathf.Max(Mathf.Min(maxSpeedY, velocity.y), -maxSpeedY);
	}

    private void TogglePause() {
        paused = !paused;
        pausedScreen.SetActive(paused);
		if (paused) { Time.timeScale=0; } 
        else { Time.timeScale=1; }  
        Debug.LogWarning("Pausing");
    }

	private void UnPause() {
        paused = false;
        // pausedScreen.SetActive(false); 
        Time.timeScale = 1;  
    }


    private void Mute()
	{
        muted = !muted;
        AudioListener.volume =  muted ? 0 : 1;
    }

	private void pauseUI()
	{
		if (pausedScreen == null || !Data.started)
		{
			return;
		}
		if(Input.GetButtonDown("TogglePause")){
            TogglePause();
        }
        if(Input.GetButtonDown("Mute")){
            Mute();
        }
        if(Input.GetButtonDown("Reset")){
			UnPause();
            SceneManager.LoadSceneAsync(homeScreen);
        }
        if(Input.GetButtonDown("Restart")){
			UnPause();
        	SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
	}
	// Update is called once per frame
	void Update () 
	{
		pauseUI();

		Vector3 input = Vector3.zero;
		if (!dead && !inactive) 
		{
			input = new Vector3(Input.GetAxisRaw("Horizontal"), 
								Input.GetAxisRaw("Vertical"),
								Input.GetAxisRaw("Jump"));

			if (stunned) { CheckUnStun(); }
			else 
			{
				Bounce(ref velocity, input);
				Move  (ref velocity, input);
			}
			Trail();
		} if (inactive) {
			if (c2d.collision.below || c2d.collision.above) 
				velocity.y = c2d.collision.below? jumpVelocity:-jumpVelocity;
		}
		ApplyGravity(ref velocity, input);
		
		velocity += addedVelocity;
		addedVelocity = Vector3.zero;

		velocity *= velocityModifier;
		velocityModifier = 1;

		ClampSpeeds(ref velocity);
		prevVelocity = velocity;
		c2d.Move(prevVelocity*Time.deltaTime);

        animor.SetFloat("vspeed", velocity.y);
	}

	private void PlayAudio(AudioClip clip)
    {
         if (playing <= 0) return;
         StartCoroutine(PlayClip(clip));
    }
	IEnumerator PlayClip(AudioClip clip)
    {
         playing--;
         audSrc.PlayOneShot(clip);
         yield return new WaitForSeconds(clip.length);
         playing++;
    }
}
