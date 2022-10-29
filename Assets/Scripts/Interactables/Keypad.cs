using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : Interactable
{


    public string code = "12345";
    public string entered_code = "";
    public AudioClip keyPress, access_denied,access_granted;
    public AudioSource audioSource;
    public Light keypad_light;
    public Animator controller;
    public string animInvoker = "";
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if(isLooking){
            GetComponent<Outline>().enabled = true;
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        }else{
            GetComponent<Outline>().enabled = false;
        }
    }

    protected override void Interact()
    {
        
    }
}
