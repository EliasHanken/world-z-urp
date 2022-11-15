using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private KeyCode pauseButton;
    public Canvas canvas;
    public Slider volumeSlider;
    public bool playerDead = false;

    private bool _pause = false;
    private float fixedDeltaTime;
    void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        if(canvas == null){
            canvas = GetComponentInChildren<Canvas>();
        }
        
    }

    void Update()
    {
        if(Input.GetKeyDown(pauseButton))
        {
            Debug.Log("Pause pressed");
            _pause = !_pause;
        }

        canvas.gameObject.SetActive(_pause);

        if(_pause){
            Time.timeScale = 0.1f;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
            //AudioListener.volume = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }else{
            if(playerDead)return;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 1f;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 1f;
            }
            foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 1f;
            }
            Time.timeScale = 1f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; 
            AudioListener.volume = volumeSlider.value;
        }

        if(volumeSlider.value < 0.01){
            AudioListener.volume = 0.0f;
        }
    }

    public bool isPaused()
    {
        return _pause;
    }
}
