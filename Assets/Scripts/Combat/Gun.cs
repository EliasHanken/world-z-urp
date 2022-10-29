using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    public float damage = 1f;
    public float headshotMultiplier = 3f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public bool automatic = false;
    public float reloadTime = 1f;
    public float reloadSoundDelay = 1f;
    public int bulletClipSize = 7;
    public int bulletsInSpare = 21;
    public ProceduralRecoil proceduralRecoil;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem impactEffectZombie;
    public ParticleSystem impactEffectOther;
    public ParticleSystem headshotEffect;
    public AudioSource fireSound;
    public Animator anim;
    public AudioClip reloadSound;
    public TextMeshProUGUI ammoUI;
    public bool isEquipped = false;

    private float nextTimeToFire = 0f;
    private bool onCooldown = false;
    private bool _ads = false;
    private float _sens;

    private int _bullets;
    private int _bulletsLeft;

    private float _fov;

    void Start(){
        if(!isEquipped) return;
        _bullets = bulletClipSize;
        _bulletsLeft = bulletsInSpare;
        _sens = gameObject.GetComponentInParent<CameraController>().mouseSensitivity;
        _fov = fpsCam.fieldOfView;

        /*
        proceduralRecoil = GetComponentInParent<ProceduralRecoil>();
        foreach(Camera cam in GetComponentsInParent<Camera>()){
            if(cam.gameObject.tag == "MainCamera"){
                fpsCam = cam;
            }
        }
        muzzleFlash = GameObject.Find("MuzzleFlashEffect").GetComponent<ParticleSystem>();
        ammoUI = GameObject.Find("AmmoUI").GetComponent<TextMeshProUGUI>();
        */
    }

    void Update()
    {
        if(!isEquipped)return;
        ammoUI.text = ammoText();
        if(Input.GetKey(KeyCode.R) && !anim.GetBool("reload"))
        {
            if(_bulletsLeft > 0)
            {
                Reload();
            }
            
        }
        if (Time.time >= nextTimeToFire)
        {
            onCooldown = false;
        }
        if(Input.GetKey(KeyCode.Mouse1)){
            _ads = true;
            gameObject.GetComponentInParent<CameraController>().mouseSensitivity = _sens * gameObject.GetComponentInParent<CameraController>().mouseADSmultiplier;
            gameObject.GetComponentInParent<PlayerMovement>().setADS = true;
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView,_fov-15,15 * Time.deltaTime);
        }else if(!Input.GetKeyDown(KeyCode.Mouse1)){
            _ads = false;
            gameObject.GetComponentInParent<CameraController>().mouseSensitivity = _sens;
            gameObject.GetComponentInParent<PlayerMovement>().setADS = false;
            anim.SetBool("ads",false);
            fpsCam.fieldOfView = Mathf.Lerp(fpsCam.fieldOfView,_fov,15 * Time.deltaTime);
        }
        if(onCooldown || anim.GetBool("reload")) return;
        if(automatic)
        {
            if(Input.GetKey(KeyCode.Mouse0) && !onCooldown)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                Shoot();
            }
        }else{
            if(Input.GetKeyDown(KeyCode.Mouse0) && !onCooldown)
            {
                nextTimeToFire = Time.time + 1f/fireRate;
                Shoot();
            }
        }
        
    }

    private void Reload()
    {
        if(_bulletsLeft <= 0) return;
        if(_bullets >= bulletClipSize)return;
        anim.SetBool("reload", true);
        onCooldown = true;
        Invoke("finishReload", reloadTime);
        Invoke("reloadSounds", reloadTime - reloadSoundDelay);
    }

    void Shoot(){
        PlayerMovement pm = gameObject.GetComponentInParent<PlayerMovement>();
        if(pm != null){
            if(pm.isJumping){
                return;
            }
        }
        if(_bullets <= 0){
            Reload();
            return;
        }
        _bullets--;
        onCooldown = true;
        anim.SetBool("shoot",true);
        CancelInvoke("resetAnim");
        Invoke("resetAnim",1f/fireRate);
        
        proceduralRecoil.recoil();
        muzzleFlash.Play();
        fireSound.PlayOneShot(fireSound.clip);
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            EntityHealth entityHealth = hit.transform.GetComponentInParent<EntityHealth>();
            

            if(entityHealth != null)
            {
                if(entityHealth.isPlayer)
                {
                    return;
                }
                if(hit.transform.tag == "EntityHead")
                {
                    if(entityHealth.head != null)
                    {
                        // Destroy(entityHealth.head);
                    }
                    if(headshotEffect != null)
                    {
                        Instantiate(headshotEffect, hit.point,Quaternion.LookRotation(hit.normal));
                    }
                    entityHealth.TakeDamage(damage * headshotMultiplier,true);
                }else
                {
                    entityHealth.TakeDamage(damage,false);
                }
                if(impactEffectZombie != null)
                {
                    Instantiate(impactEffectZombie, hit.point, Quaternion.LookRotation(hit.normal));
                    
                }
            }else
            {
                if(hit.collider.tag != "NoCollision"){
                    Instantiate(impactEffectOther, hit.point, Quaternion.LookRotation(hit.normal));
                }
                
            }

            Zombie zombie = hit.transform.GetComponentInParent<Zombie>();
            if(zombie != null){
                Vector3 forceDirection = zombie.transform.position - fpsCam.transform.position;
                forceDirection.y = 0.25f;
                forceDirection.Normalize();

                Vector3 force = impactForce * forceDirection;
            
                if(entityHealth.health <= 0){
                    zombie.TriggerRagdoll(force/30,hit.point);
                }
            }
            
            

            if(hit.rigidbody != null && hit.transform.GetComponentInParent<Zombie>() == null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
    void resetAnim(){
        anim.SetBool("shoot",false);
    }

    void finishReload(){
        anim.SetBool("reload",false);
        onCooldown = false;
        int bulletsToAdd = bulletClipSize - _bullets;
        if(_bulletsLeft >= bulletsToAdd){
            _bullets += bulletsToAdd;
            _bulletsLeft -= bulletsToAdd;
        }else{
            _bullets += _bulletsLeft;
            _bulletsLeft -= _bulletsLeft;
        }
    }

    void reloadSounds(){
        fireSound.PlayOneShot(reloadSound);
    }

    string ammoText(){
        return _bullets + "/" + _bulletsLeft;
    }

    public void addBullets(int bullets){
        _bulletsLeft += bullets;
    }
}


