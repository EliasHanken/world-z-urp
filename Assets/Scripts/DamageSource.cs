using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private bool _isCausingDamage = false;

    public float DamageRepeatRate = 1f;
    public int DamageAmount = 1;
    public bool Repeating = true;
    public float DamageCooldown = 1f;

    public Collider otherCol; // FIX LIST
    private void OnTriggerEnter(Collider other){
        otherCol = other;
        if(Repeating){
            InvokeRepeating("TakeDamage",DamageRepeatRate,DamageRepeatRate);
            _isCausingDamage = true;
        }else{
            Invoke("TakeDamage",0f);
            
        }
    }

    private void damageCooldown(){
        gameObject.GetComponent<Collider>().enabled = true;
    }

    private void Update(){
        if(GetComponentInParent<EntityHealth>() != null){
            EntityHealth eh = GetComponentInParent<EntityHealth>();
            if(!eh.isPlayer){
                if(eh.hasDied){
                    gameObject.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }


    private void OnTriggerExit(Collider other){
        otherCol = other;
        _isCausingDamage = false;
        CancelInvoke("TakeDamage");

        Invoke("damageCooldown",DamageCooldown);
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // Update is called once per frame
    private void TakeDamage(){
        if(otherCol == null) return;
        EntityHealth entityHealth = otherCol.GetComponent<EntityHealth>();
        if(entityHealth != null){
            entityHealth.TakeDamage(DamageAmount,false);
        }
    }
}
