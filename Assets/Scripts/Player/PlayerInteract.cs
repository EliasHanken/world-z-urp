using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private GameObject lastLookObject;
    void Start()
    {
        cam = GetComponent<PlayerMovement>().cam;
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, distance,mask)){
            if(hitInfo.collider.GetComponent<Interactable>() != null){
                lastLookObject = hitInfo.collider.gameObject;
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.promptMessage);
                interactable.isLooking = true;
                interactable.whoIsLooking = gameObject;
                if(Input.GetKeyDown(KeyCode.F)){
                    interactable.BaseInteract();
                }
            }
        }else{
            if(lastLookObject != null){
                lastLookObject.GetComponent<Interactable>().isLooking = false;
                lastLookObject.GetComponent<Interactable>().whoIsLooking = null;
            }
        }
    }
}
