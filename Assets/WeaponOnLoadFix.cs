using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponOnLoadFix : MonoBehaviour
{
    [SerializeField]
    private GameObject[] gameObjects;
    void Start()
    {
        foreach(GameObject ob in gameObjects){
            //gameObject.SetActive(false);
            //gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
