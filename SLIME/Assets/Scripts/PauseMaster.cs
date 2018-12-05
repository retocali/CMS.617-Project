using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMaster : MonoBehaviour 
{
    public AudioClip pauseIn;
    public AudioClip pauseOut;
    public AudioClip select;
    private AudioSource audsrc;

    public GameObject pausedScreen;
    public GameObject PauseButton;
    public GameObject MuteButton;
    public GameObject RestartButton;
    public GameObject QuitButton;
    public GameObject ExitButton;
    public GameObject CreditsButton = null;
    public GameObject[] buttons;

    public int index = 0;
    
    private float time = 0;
    private float last = 0;
    private float wait = 0.25f;
    private float vol = 0.5f;

    private bool paused = false;
    private string homeScreen = "hub-world";
    private string muteOn = "Mute: <b><color='#ffffff'>On</color></b>";
    private string muteOff = "Mute: <b><color='#000000'>Off</color></b>";

	// Use this for initialization
	void Start () 
	{
        index = 0;
        AudioListener.volume =  Data.muted ? 0 : 1;
        MuteButton.GetComponentInChildren<Text>().text = Data.muted ? muteOn : muteOff;
        audsrc = GetComponent<AudioSource>();
	}

    public void TogglePause() {
        paused = !paused;
        pausedScreen.SetActive(paused);
		
        if (paused) 
        { 
            audsrc.PlayOneShot(pauseIn, vol);
            Time.timeScale=0; 
        } 
        else 
        { 
            audsrc.PlayOneShot(pauseOut, vol);
            Time.timeScale=1; 
        }  
        last = 0;
        time = 0;
        index = 0;
        MusicMaster.toggleBackground();
    }

	private void UnPause() {
        index = 0;
        paused = false;
        Time.timeScale = 1;  
        pausedScreen.SetActive(false); 
    }


    private void Mute()
	{
        Data.muted = !Data.muted;
        AudioListener.volume =  Data.muted ? 0 : 1;
    }

	void Update()
	{
		if (pausedScreen == null || !Data.started)
		{
			return;
		}
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 
								Input.GetAxisRaw("Vertical"),
								Input.GetAxisRaw("Jump"));

        if(Input.GetButtonDown("TogglePause") || PauseButton.GetComponent<PauseButtonScript>().pressed)
        {
            TogglePause();
            PauseButton.GetComponent<PauseButtonScript>().pressed = false;
        }
        if (!paused) { return;}
        
        if (time < wait) 
        { 
            time += Time.realtimeSinceStartup-last; 
        } 
        else 
        {   
            if (input.y > 0) {
                index = (index - 1 + buttons.Length) % buttons.Length;
                buttons[index].GetComponent<PauseButtonScript>().Select();
                time = 0;
            }
            if (input.y < 0) {
                index = (index + 1) % buttons.Length;
                buttons[index].GetComponent<PauseButtonScript>().Select();
                time = 0;
            }
            if (input.z != 0)
            {
                buttons[index].GetComponent<PauseButtonScript>().pressed = true;
                time = 0;
            }
            
        }


        if(MuteButton.GetComponent<PauseButtonScript>().pressed)
        {
            Mute();
            Debug.Log("Toggling Mute");
            MuteButton.GetComponent<PauseButtonScript>().pressed = false;
            MuteButton.GetComponentInChildren<Text>().text = Data.muted ? muteOn : muteOff;
        }
        if(RestartButton.GetComponent<PauseButtonScript>().pressed)
        {
         	UnPause();
            Debug.Log("Restarting");
            RestartButton.GetComponent<PauseButtonScript>().pressed = false;
        	SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
        if(QuitButton.GetComponent<PauseButtonScript>().pressed)
        {
         	UnPause();
            Debug.Log("Quitting");
            QuitButton.GetComponent<PauseButtonScript>().pressed = false;
            SceneManager.LoadSceneAsync(homeScreen);
        }
        if (ExitButton.GetComponent<PauseButtonScript>().pressed) {
            Debug.Log("See ya nerds!");
            ExitButton.GetComponent<PauseButtonScript>().pressed = false;
            Application.Quit();
        }
        if (CreditsButton != null)
        {
            if (CreditsButton.GetComponent<PauseButtonScript>().pressed) {
            
            CreditsButton.GetComponent<PauseButtonScript>().pressed = false;
            SceneManager.LoadSceneAsync("EndGame");
        }
        }
        
        last = Time.realtimeSinceStartup;
	}
    public void DeselectAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<PauseButtonScript>().Deselect();
        }
    }
    public void Select(int i)
    {
        DeselectAll();
        index = i;
        audsrc.PlayOneShot(select, vol);
    }
}
