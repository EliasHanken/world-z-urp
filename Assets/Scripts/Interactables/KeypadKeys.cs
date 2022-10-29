using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadKeys : Interactable
{
    public Keypad keypad;
    void Update()
    {
        if(isLooking){
            GetComponent<Outline>().enabled = true;
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        }else{
            GetComponent<Outline>().enabled = false;
        }
    }

    void Start()
    {
        keypad.keypad_light.color = Color.red;
        if(keypad.controller != null)
        {
            keypad.controller.SetBool(keypad.animInvoker,false);
        }
        
    }

    protected override void Interact()
    {
        // Play sound ...
        if(promptMessage == "Enter"){
            if(keypad.entered_code == keypad.code){
                Debug.Log("Door opened from keypad");
                keypad.entered_code = "";
                keypad.audioSource.PlayOneShot(keypad.access_granted);
                keypad.keypad_light.color = Color.green;

                if(keypad.controller != null){
                    keypad.controller.SetBool(keypad.animInvoker,true);
                }
                
            }else{
                keypad.entered_code = "";
                Debug.Log("Wrong code");
                keypad.audioSource.PlayOneShot(keypad.access_denied);
            }
        }else{
            // Play sound ...
            keypad.entered_code += promptMessage;
            keypad.audioSource.PlayOneShot(keypad.keyPress);
        }
    }
}
