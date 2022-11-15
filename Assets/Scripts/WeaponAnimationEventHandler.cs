using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;
    private AudioSource audioSource;
    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        audioSource = weapon.GetComponent<AudioSource>();
    }

    public void WeaponReloadFinish(){
        weapon.WeaponReloadFinish();
    }

    public void ClipTakeoutSound(){
        audioSource.PlayOneShot(weapon.soundClipTakeout);
    }

    public void ClipInsertSound(){
        audioSource.PlayOneShot(weapon.soundClipInsert);
    }

    public void ClipReloadSound(){
        audioSource.PlayOneShot(weapon.soundClipReload);
    }
}
