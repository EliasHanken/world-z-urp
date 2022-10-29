using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    public int capacity = 1;
    [SerializeField]
    private KeyCode openKey = KeyCode.E;
    [SerializeField]
    public List<GameObject> items;
    private bool isOpen = false;

    void Start(){
        items = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOpen)
        {
            if(Input.GetKey(openKey))
            {
                Debug.Log("Inventory opened");
                isOpen = true;
            }
        }else{
            if(Input.GetKey(openKey))
            {
                Debug.Log("Inventory closed");
                isOpen = false;
            }
        }
    }
}
