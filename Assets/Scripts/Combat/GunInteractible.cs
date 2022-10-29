using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteractible : MonoBehaviour
{
    public float interactibleDistance = 2f;
    public RuntimeAnimatorController animatorController;
    public Animator parentAnimator;
    public Avatar avatar;
    void Start()
    {
        if(GetComponentInParent<Animator>() != null){
            parentAnimator = GetComponentInParent<Animator>();
            avatar = parentAnimator.avatar;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponentInParent<Animator>() == null){
            parentAnimator = null;
            avatar = null;
        }
    }
}
