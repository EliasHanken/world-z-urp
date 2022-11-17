using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    public enum GunType{
        Pistol,Smg, Rifle, Sniper
    }
    private float _sens;
    [Header("Audio Settings")]
    [SerializeField]
    private AudioMixerGroup audioMixerGroup;

    [Header("Dependent Objects")]
    [SerializeField]
    private Camera fpsCam;

    [Header("Presets (Not Setup)")]
    [SerializeField]
    private GunType preset;

    [Header("Impact Settings")]
    [SerializeField]
    private AudioClip fleshHit;
    [SerializeField]
    private AudioClip stoneHit;
    [SerializeField]
    private AudioClip headshotSound;

    [Header("Gun Settings")]
    [SerializeField]
    private string gunName;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int defaultAmmoSpare;
    [SerializeField]
    private int clipSize;
    [SerializeField]
    private int currentAmmo;
    [SerializeField]
    private int currentAmmoInMagazine;
    [SerializeField]
    private float impactForce;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float headshotMultiplier;
    [SerializeField]
    private GunType ammoType;
    [SerializeField]
    private GunType gunType;
    private Image gunIcon;
    [SerializeField]
    private AudioClip fireSound;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform muzzlePos;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private ParticleSystem headshotEffect;
    [SerializeField]
    private ParticleSystem impactEffectOther;
    [SerializeField]
    private ParticleSystem impactEffectFlesh;
    [SerializeField]
    private Transform transformHolder;
    [SerializeField]
    public AudioClip soundClipTakeout;
    [SerializeField]
    public AudioClip soundClipInsert;
    [SerializeField]
    public AudioClip soundClipReload;
    [SerializeField]
    public AudioClip soundClipNoAmmoShoot;

    // Private local stuff

    private bool _fireCooldown;
    private float _nextTimeToFire = 0f;

    private Vector3 staticAdsPos = new Vector3(-0.047f,-0.06f,-0.21f);
    private Vector3 staticDefaultPos = new Vector3(0.06f,-0.12f,-0.13f);

    private List<ParticleSystem> muzzleFlashList;
    private List<ParticleSystem> impactEffects;

    private ProceduralRecoil proceduralRecoil;

    private TextMeshProUGUI ammoGui;

    private float _currentCHairAlpha = 255f;
    

    void Start()
    {
        _sens = GetComponentInParent<CameraController>().mouseSensitivity;

        ammoGui = GameObject.FindGameObjectWithTag("AmmoHud").GetComponent<TextMeshProUGUI>();

        currentAmmo = defaultAmmoSpare;
        currentAmmoInMagazine = clipSize;

        _fireCooldown = false;
        muzzleFlashList = new List<ParticleSystem>();
        impactEffects = new List<ParticleSystem>();

        proceduralRecoil = GetComponentInParent<ProceduralRecoil>();

        //Disable crosshair for more realistic look
        
    }



    void Update()
    {
        if(GameObject.FindGameObjectWithTag("GameManager") == null) return;
        // Update hud
        ammoGui.text = currentAmmoInMagazine + "|" + currentAmmo;
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Arm")){
            return;
        }
        if(Input.GetKeyDown(KeyCode.R)){
            Reload();
        }
        
        if(Input.GetKey(KeyCode.Mouse1)){
            if(animator.GetBool("Reload"))return; // Return if reload so player can't ads at the same time
            adsRecoil();
            Vector3 newPos = Vector3.Lerp(transformHolder.localPosition,staticAdsPos,10 * Time.deltaTime);
            transformHolder.localPosition = newPos;

            
            DynamicCrosshair dynamicCrosshair = GameObject.FindGameObjectWithTag("CrosshairManager").GetComponent<DynamicCrosshair>();
            if(dynamicCrosshair != null){
                _currentCHairAlpha = Mathf.Lerp(_currentCHairAlpha,0,40 * Time.deltaTime);
                foreach(Image img in dynamicCrosshair.GetComponentsInChildren<Image>()){
                    Color cc = img.color;
                    
                    cc.a = _currentCHairAlpha;
                    img.color = cc;
                }    
            }
            
        }else{
            setRecoilFull();
            Vector3 newPos = Vector3.Lerp(transformHolder.localPosition,staticDefaultPos,10 * Time.deltaTime);
            transformHolder.localPosition = newPos;

    
            DynamicCrosshair dynamicCrosshair = GameObject.FindGameObjectWithTag("CrosshairManager").GetComponent<DynamicCrosshair>();
            if(dynamicCrosshair != null){
                _currentCHairAlpha = Mathf.Lerp(_currentCHairAlpha,255,40 * Time.deltaTime);
                foreach(Image img in dynamicCrosshair.GetComponentsInChildren<Image>()){
                    Color cc = img.color;
                    
                    cc.a = _currentCHairAlpha;
                    img.color = cc;
                }  
            }
            
        }
        // Reset cooldown if time bigger than next time to fire.
        if (Time.time >= _nextTimeToFire)
        {
            _fireCooldown = false;
        }

        if(gunType == GunType.Sniper || gunType == GunType.Pistol) // Non automatic
        {
            if(Input.GetKeyDown(KeyCode.Mouse0) && !_fireCooldown)
            {
                _nextTimeToFire = Time.time + 1f/fireRate;
                Shoot();
            }
        }else{
            if(Input.GetKey(KeyCode.Mouse0) && !_fireCooldown) // Automatic
            {
                _nextTimeToFire = Time.time + 1f/fireRate;
                Shoot();
            }
        }

        
    }

    void FixedUpdate(){
        // Update muzzle flas position
        foreach(ParticleSystem particleSystem in muzzleFlashList){
            particleSystem.transform.position = muzzlePos.position;
            particleSystem.transform.rotation = muzzlePos.rotation;
        }
    }
    
    // Animation Event
    public void WeaponReloadFinish(){
        animator.SetBool("Reload",false);
        if(currentAmmoInMagazine == 0){
            if(currentAmmo >= clipSize){
                currentAmmoInMagazine += clipSize;
                currentAmmo -= clipSize;
            }else{
                currentAmmoInMagazine += currentAmmo;
                currentAmmo = 0;
            }
            
        }else{
            int ammoNeeded = clipSize - currentAmmoInMagazine;
            if(currentAmmo >= ammoNeeded){
                currentAmmo -= ammoNeeded;
                currentAmmoInMagazine += ammoNeeded;
            }else{
                currentAmmoInMagazine += currentAmmo;
                currentAmmo = 0;
            }
            
        }
    }

    private void Reload(){
        if(currentAmmoInMagazine < clipSize && currentAmmo > 0){
            animator.SetBool("Reload",true);
        }
    }

    private void Shoot(){
        // Check if paused.
        if(GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().isPaused()) return;
        // Check if ammo is 0.
        if(currentAmmoInMagazine <= 0){
            GetComponent<AudioSource>().PlayOneShot(soundClipNoAmmoShoot);
            _fireCooldown = true;
            return;
        }

        PlayerMovement pm = gameObject.GetComponentInParent<PlayerMovement>();
        if(pm != null){
            if(pm.isJumping){
                return;
            }
        }

        currentAmmoInMagazine--;

        GetComponent<AudioSource>().PlayOneShot(fireSound);
        GetComponentInParent<ProceduralRecoil>().recoil();
        ParticleSystem fireEffect = Instantiate(muzzleFlash,muzzlePos.transform.position,Quaternion.Euler(muzzlePos.forward));
        muzzleFlashList.Add(fireEffect);
        _fireCooldown = true;
        StartCoroutine(waitAndDestroyMuzzleFlash(fireEffect));

        RaycastHit hit;
        int layerMask = 1 << 9;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 300,layerMask,QueryTriggerInteraction.Ignore))
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
                        GameObject instantiatedAS = new GameObject();
                        instantiatedAS.AddComponent<AudioSource>();
                        AudioSource audioSource = instantiatedAS.GetComponent<AudioSource>();
                        
                        audioSource.pitch = 0.8f;
                        audioSource.spatialBlend = 1.0f;
                        audioSource.transform.position = hit.transform.position;
                        audioSource.outputAudioMixerGroup = audioMixerGroup;
                        audioSource.clip = headshotSound;
                        audioSource.Play();
                        StartCoroutine(DestroyComponent(audioSource));

                        //ParticleSystem ps = Instantiate(headshotEffect, hit.point,Quaternion.LookRotation(hit.normal));
                        //waitAndDestroyImpact(ps);

                    }
                    entityHealth.TakeDamage(damage * headshotMultiplier,true);
                }else
                {
                    entityHealth.TakeDamage(damage,false);
                }
                if(impactEffectFlesh != null)
                {
                    ParticleSystem ps = Instantiate(impactEffectFlesh, hit.point, Quaternion.LookRotation(hit.normal));
                    waitAndDestroyImpact(ps);
                }
            }else
            {
                if(hit.collider.tag != "NoCollision"){
                    ParticleSystem ps = Instantiate(impactEffectOther, hit.point, Quaternion.LookRotation(hit.normal));
                    waitAndDestroyImpact(ps);
                }
                
            }

            Zombie zombie = hit.transform.GetComponentInParent<Zombie>();
            if(zombie != null){
                GameObject instantiatedAS = new GameObject();
                AudioSource audioSource = instantiatedAS.AddComponent<AudioSource>();
                audioSource.transform.position = hit.transform.position;
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.PlayOneShot(fleshHit);
                StartCoroutine(DestroyComponent(audioSource));

                Vector3 forceDirection = zombie.transform.position - fpsCam.transform.position;
                forceDirection.y = 0.25f;
                forceDirection.Normalize();

                Vector3 force = impactForce * forceDirection;

                if(entityHealth.health > 0){
                    //fastToggleRagdoll(zombie,force/30,hit.point);
                    //zombie.TriggerShot();
                }
            
                if(entityHealth.health <= 0){
                    zombie.TriggerRagdoll(force/30,hit.point);
                    if(zombie.isDead) return;
                    if(zombie.canBeReborn){
                        if(zombie._reborn){
                            GameObject obj = GameObject.FindGameObjectWithTag("ObjectiveHandler");
                            ObjectiveHandler objectiveHandler = obj.GetComponent<ObjectiveHandler>();

                            List<GameObject> kill_list_obj = objectiveHandler.getObjective(ObjectiveHandler.ObjectiveType.entity_kills);
                                foreach(GameObject go in kill_list_obj){
                                    if(go.GetComponent<ObjectiveKill>() != null){
                                        go.GetComponent<ObjectiveKill>().increaseProgressByOne();
                                    }
                                    
                            }
                        }
                    }else{
                        if(!zombie.isDead){
                            GameObject obj = GameObject.FindGameObjectWithTag("ObjectiveHandler");
                            ObjectiveHandler objectiveHandler = obj.GetComponent<ObjectiveHandler>();

                            List<GameObject> kill_list_obj = objectiveHandler.getObjective(ObjectiveHandler.ObjectiveType.entity_kills);
                            foreach(GameObject go in kill_list_obj){
                                go.GetComponent<ObjectiveKill>().increaseProgressByOne();
                            }
                        }
                    }

                    
                    
                }
            }
            
            

            if(hit.rigidbody != null && hit.transform.GetComponentInParent<Zombie>() == null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }

    IEnumerator waitAndDestroyMuzzleFlash(ParticleSystem _object){
        yield return new WaitForSeconds(0.5f);
        Destroy(_object);
        muzzleFlashList.Remove(_object);
    }

    IEnumerator waitAndDestroyImpact(ParticleSystem _object){
        yield return new WaitForSeconds(5f);
        impactEffects.Remove(_object);
        Destroy(_object);
    }

    IEnumerator waitAndDestroy(GameObject _object){
        yield return new WaitForSeconds(0.5f);
        Destroy(_object);
    }

    IEnumerator DestroyComponent(AudioSource audioSource){
        yield return new WaitForSeconds(1f);
        Destroy(audioSource);
    }

    void setRecoilFull(){
        proceduralRecoil.recoilX = -10.7f;
        proceduralRecoil.recoilY = 5.5f;
        proceduralRecoil.recoilZ = 0.2f;

        proceduralRecoil.kickBackZ = 0.2f;
        proceduralRecoil.snappiness = 10.36f;
        proceduralRecoil.returnAmount = 20f;
    }

    void adsRecoil(){
        proceduralRecoil.recoilX = 1f;
        proceduralRecoil.recoilY = 1f;
        proceduralRecoil.recoilZ = 0.2f;

        proceduralRecoil.kickBackZ = 0.15f;
        proceduralRecoil.snappiness = 8f;
        proceduralRecoil.returnAmount = 15f;
    }
}
