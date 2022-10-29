using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject rifle, pistol, knife;

    void Start()
    {
        rifle.SetActive(true);
        pistol.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1)){
            if(rifle != null)
            {
                rifle.SetActive(true);

                pistol.SetActive(false);
                //knife.SetActive(true);
            }
        }else if(Input.GetKey(KeyCode.Alpha2)){
            if(pistol != null)
            {
                pistol.SetActive(true);

                rifle.SetActive(false);
                //knife.SetActive(false);
            }
        }else if(Input.GetKey(KeyCode.Alpha3)){
            if(knife != null)
            {
                knife.SetActive(true);

                pistol.SetActive(true);
                rifle.SetActive(true);
            }
        }
    }
}
