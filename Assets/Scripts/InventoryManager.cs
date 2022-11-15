using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> inventoryList;
    public List<Weapon> weaponList;
    void Start()
    {
        inventoryList = new List<GameObject>();
        weaponList = new List<Weapon>();
    }
    
    void Update()
    {
        
    }
}
