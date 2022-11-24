using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private KeyCode pauseButton;
    public GameObject pauseObject;
    public GameObject copyright;
    public Slider volumeSlider;
    public GameObject missionComplete;
    public bool playerDead = false;
    public string scene_level_name;

    private bool _pause = false;
    public bool _override = false;
    private float fixedDeltaTime;

    private float pausedZombieAudio;
    private float pausedPlayerAudio;
    private float pausedEnvironmentAudio;
    void Start()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                pausedZombieAudio = audioSource.volume;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                pausedEnvironmentAudio = audioSource.volume;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                pausedPlayerAudio = audioSource.volume;
            }
    }

    void Update()
    {
        if(missionComplete.activeInHierarchy)return;
        if(Input.GetKeyDown(pauseButton))
        {
            if(_pause && !pauseObject.activeInHierarchy) return;
            Debug.Log("Pause pressed");
            _pause = !_pause;
            pauseObject.SetActive(_pause);
            copyright.SetActive(_pause);
        }
        if(_pause){
            Time.timeScale = 0.0f;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
                //AudioSource audioSource = go.GetComponent<AudioSource>();
                //audioSource.volume = 0.0f;
                go.GetComponent<AudioSource>().enabled = false;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
                //AudioSource audioSource = go.GetComponent<AudioSource>();
                //audioSource.volume = 0.0f;
                go.GetComponent<AudioSource>().enabled = false;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                //AudioSource audioSource = go.GetComponent<AudioSource>();
                //audioSource.volume = 0.0f;
                go.GetComponent<AudioSource>().enabled = false;
            }
            //AudioListener.volume = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }else if(!_pause){
            if(playerDead)return;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
                //AudioSource audioSource = go.GetComponent<AudioSource>();
                //audioSource.volume = pausedZombieAudio;
                go.GetComponent<AudioSource>().enabled = true;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
                //AudioSource audioSource = go.GetComponent<AudioSource>();
                //audioSource.volume = pausedEnvironmentAudio;
                go.GetComponent<AudioSource>().enabled = true;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                //AudioSource audioSource = go.GetComponent<AudioSource>();
                //audioSource.volume = pausedPlayerAudio;
                go.GetComponent<AudioSource>().enabled = true;
            }
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
            //AudioListener.volume = volumeSlider.value;
        }

        //if(volumeSlider.value < 0.01){
        //    AudioListener.volume = 0.0f;
        //}
    }

    public bool isPaused()
    {
        return _pause;
    }

    public void restartScene(){
        SceneManager.LoadScene(scene_level_name);
    }

    public void loadScene(string name){
        SceneManager.LoadScene(name);
    }
}
