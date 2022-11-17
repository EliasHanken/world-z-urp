using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventoryList;
    public List<Weapon> weaponList;
    public GameObject rifle;
    public GameObject rifleArmMesh;
    public GameObject melee;
    public GameObject meleeArmMesh;
    public float switchDelay = 0.5f;
    public float timer;
    public bool _switch = false;
    public int switchIndex = 3;
    void Start()
    {
        inventoryList = new List<GameObject>();
        weaponList = new List<Weapon>();
        timer = switchDelay;
    }
    
    void Update()
    {
        /*
        if(_switch){
            timer -= Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            melee.GetComponentInParent<Animator>().SetBool("Unarm",true);
            melee.GetComponent<Weapon>().enabled = false;

            switchIndex = 1;
            _switch = true;
            
            if(timer < 0 && switchIndex == 1){
                rifle.GetComponent<Weapon>().enabled = true;
                rifle.SetActive(true);
                rifleArmMesh.SetActive(true);
                rifle.GetComponentInParent<Animator>().SetBool("Unarm",false);
                timer = switchDelay;
                _switch = false;
            }
            
        }else if(Input.GetKeyDown(KeyCode.Alpha3)){
            rifle.GetComponentInParent<Animator>().SetBool("Unarm",true);
            rifle.GetComponent<Weapon>().enabled = false;

            switchIndex = 3;
            _switch = true;

            if(timer < 0 && switchIndex == 3){
                melee.GetComponent<Weapon>().enabled = true;
                melee.SetActive(true);
                meleeArmMesh.SetActive(true);
                melee.GetComponentInParent<Animator>().SetBool("Unarm",false);
                timer = switchDelay;
                _switch = false;
            }
            
        }
        */

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            if(!melee.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))return;
            melee.GetComponentInParent<Animator>().SetBool("Unarm",true);
            melee.GetComponent<Weapon>().enabled = false;

            
            rifle.GetComponent<Weapon>().enabled = true;
            rifle.SetActive(true);
            rifleArmMesh.SetActive(true);
            rifle.GetComponentInParent<Animator>().SetBool("Unarm",false);
            rifle.GetComponentInParent<Animator>().SetBool("Arm",true);
            
        }else if(Input.GetKeyDown(KeyCode.Alpha3)){
            if(!rifle.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))return;
            rifle.GetComponentInParent<Animator>().SetBool("Unarm",true);
            rifle.GetComponent<Weapon>().enabled = false;

            
            melee.GetComponent<Weapon>().enabled = true;
            melee.SetActive(true);
            meleeArmMesh.SetActive(true);
            melee.GetComponentInParent<Animator>().SetBool("Unarm",false);
            melee.GetComponentInParent<Animator>().SetBool("Arm",true);
            
        }
    }
}
