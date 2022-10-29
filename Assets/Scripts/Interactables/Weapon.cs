using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable
{
    [SerializeField]
    private GameObject playerHandContainer;
    [SerializeField]
    private int amount = 10;

    public float rotateSpeed = 1f;
    public float floatFrequency, floatAmplitude = 0.25f;
    public Vector3 startPos;
    public GameObject prefab;
    void Start()
    {
        startPos = transform.position;
        promptMessage += " x" + amount;
    }

    // Update is called once per frame
    void Update()
    {
        if(isLooking){
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineVisible;
        }else{
            GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineHidden;
        }
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
 
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
 
        transform.position = tempPos;
    }

    protected override void Interact()
    {
        Inventory inv = whoIsLooking.GetComponentInChildren<Inventory>();
        if(inv != null){
            if(inv.capacity > inv.items.Count){
                inv.items.Add(prefab);
                Destroy(gameObject);

                if(inv.items.Count > 0){
                    for(var i = 0; i < inv.items.Count; i++){
                        GameObject newItem = Instantiate(inv.items[i],inv.gameObject.transform.position,inv.gameObject.transform.rotation);
                        newItem.transform.parent = inv.gameObject.transform;
                        if(newItem.GetComponent<Gun>() != null){
                            newItem.GetComponent<Gun>().isEquipped = true;
                        }
                    }
                }
            }
        }
        
    }
}
