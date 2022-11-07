using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostage : Interactable
{
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
       List<GameObject> intr_list_obj = GameObject.FindGameObjectWithTag("ObjectiveHandler").GetComponent<ObjectiveHandler>().getObjective(ObjectiveHandler.ObjectiveType.interactable);
                                foreach(GameObject go in intr_list_obj){
                                go.GetComponent<ObjectiveInteract>().finish();
                            }
        Destroy(gameObject);
    }
}
