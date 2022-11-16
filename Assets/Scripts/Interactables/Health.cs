using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Interactable
{
    [SerializeField]
    private GameObject playerHandContainer;
    [SerializeField]
    private int amount = 2;

    public float rotateSpeed = 1f;
    public float floatFrequency, floatAmplitude = 0.25f;
    public Vector3 startPos;
    void Start()
    {
        startPos = transform.position;
        promptMessage += " x" + amount;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLooking){
            GetComponent<Outline>().enabled = true;
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        }else{
            GetComponent<Outline>().enabled = false;
        }
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
 
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
 
        transform.position = tempPos;
    }

    protected override void Interact()
    {
        if(whoIsLooking.GetComponentInChildren<EntityHealth>() != null){
            whoIsLooking.GetComponentInChildren<EntityHealth>().giveHealth(amount);
            Destroy(gameObject);
        }
        
    }

}
