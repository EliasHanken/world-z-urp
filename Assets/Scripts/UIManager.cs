using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private KeyCode pauseButton;
    private Canvas canvas;
    public Slider volumeSlider;

    private bool _pause = false;
    private float fixedDeltaTime;
    void Awake()
    {
        fixedDeltaTime = Time.fixedDeltaTime;
        canvas = GetComponentInChildren<Canvas>();
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
            AudioListener.volume = 0.0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; 
        }else{
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
