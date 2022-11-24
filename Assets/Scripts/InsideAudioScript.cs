using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideAudioScript : MonoBehaviour
{

    public enum State{
        None, Enter, Exit
    }

    [SerializeField]
    private float time;
    private AudioSource audioSource;
    private AudioLowPassFilter lowPassFilter;
    public State state;


    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        lowPassFilter = GetComponentInParent<AudioLowPassFilter>();

        lowPassFilter.cutoffFrequency = 22000f;
        state = State.None;
    }

    void Update(){
        switch(state){
            case State.None:
                break;
            case State.Enter:
                enterLerp();
                break;
            case State.Exit:
                exitLerp();
                break;
        }
    }

    void enterLerp(){
        float newFreq = Mathf.Lerp(lowPassFilter.cutoffFrequency,300,time*Time.deltaTime);
        lowPassFilter.cutoffFrequency = newFreq;
    }

    void exitLerp(){
        float newFreq = Mathf.Lerp(lowPassFilter.cutoffFrequency,22000,time*Time.deltaTime);
        lowPassFilter.cutoffFrequency = newFreq;
    }

    private void OnTriggerEnter(Collider other){
        state = State.Enter;
    }

    private void OnTriggerExit(Collider other){
        state = State.Exit;
    }
}
