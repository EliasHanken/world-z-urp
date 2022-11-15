using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    [SerializeField]
    public float health = 20f;
    public bool isPlayer = false;
    public ParticleSystem deathEffect;
    public float deathTime = 3f;
    public GameObject head;
    public List<GameObject> meshToDestroy;
    float countdown;
    public bool hasDied = false;
    bool effectPlayed = false;
    public Slider healthBar;
    public AudioSource hurt_sound;
    private GameObject target;
    public Image damageOverlay;
    Rigidbody[] rigidbodies;
    Collider[] colliders;

    private float currentVelocity;
    private Color currentColor;



    void Start(){
        if(isPlayer){
            Color tempColor = damageOverlay.color;
            tempColor.a = 0f;
            damageOverlay.color = tempColor;
        }
        
        countdown = deathTime;

        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();

        if(GetComponent<Zombie>() != null) return;
        if(!isPlayer){
            foreach (Rigidbody rb in rigidbodies){
                rb.isKinematic = true;
            }
        }
    }

    void Update(){
        if(GetComponent<Zombie>()){
            return;
        }
        if(health <= 0){
            if(!isPlayer){
                countdown -= Time.deltaTime;
                if(countdown <= 0f && hasDied){
                    Destroy(gameObject); 
                }
                if(!hasDied)
                {
                    Die();
                }
            }else{
                float currentValue = Mathf.SmoothDamp(healthBar.value, 0,ref currentVelocity, 20*Time.deltaTime);
                healthBar.value = currentValue;
                PlayerDie();
            }
            
        }
        if(isPlayer){
            float currentValue = Mathf.SmoothDamp(healthBar.value, health,ref currentVelocity, 20*Time.deltaTime);
            healthBar.value = currentValue;
        }
    }

    private void Die()
    {
        hasDied = true;
        if(GetComponent<Zombie>() != null) return;
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
        gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        gameObject.GetComponent<AudioSource>().enabled = false;
        gameObject.GetComponentInChildren<Animator>().enabled = false;
        foreach (GameObject item in meshToDestroy)
        {
            Destroy(item);
        }
    }

    private void PlayerDie(){
        if(GameObject.FindGameObjectWithTag("GameManager") == null) return;
        GameObject.FindGameObjectWithTag("GameManager").SetActive(false);
        //GameObject.FindGameObjectWithTag("Player").transform.Find("WeaponRenderer").GetComponent<Camera>().enabled = false;
        GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().playerDead = true;
        GameObject.FindGameObjectWithTag("UIManager").transform.Find("DeathScreen").gameObject.SetActive(true);

        // Pause Menu effects
        Time.timeScale = 0.1f;
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Zombie")){
            AudioSource audioSource = go.GetComponent<AudioSource>();
            audioSource.volume = 0.0f;
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("EnvironmentSounds")){
            AudioSource audioSource = go.GetComponent<AudioSource>();
            audioSource.volume = 0.0f;
        }
        foreach(GameObject go in GameObject.FindGameObjectsWithTag("Player")){
                AudioSource audioSource = go.GetComponent<AudioSource>();
                audioSource.volume = 0.0f;
            }
        //AudioListener.volume = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; 
    }

    IEnumerator KillEntity(){
        yield return new WaitForSeconds(deathTime);

        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount, bool headshot){
        health -= damageAmount;
        if(hurt_sound != null){
            hurt_sound.Play();
            //Color current = damageOverlay.color;
            float alpha = Mathf.Abs((health/20)-1);
            //Debug.Log(alpha);
            //current.a = alpha;
            //damageOverlay.color = current;
            StartCoroutine(instantiateDamageOverlay(alpha,0.5f));
        }
        if(!isPlayer){
            if(headshot){
                enemyHitHeadshotEffect();
            }else{
                enemyHitEffect();
            }
        }else{
            
        }
    }
    private void enemyHitHeadshotEffect(){
        // disable movement
        CancelInvoke("stopEnemyHitFunc");
        gameObject.GetComponentInChildren<Animator>().SetBool("shot",true); 
        gameObject.GetComponentInChildren<Animator>().SetBool("isAttacking",false);
        Invoke("stopEnemyHitFunc",1.5f);
    }
    private void enemyHitEffect(){
        // disable movement
        CancelInvoke("stopEnemyHitFunc");
        gameObject.GetComponentInChildren<Animator>().SetBool("shot",true); 
        gameObject.GetComponentInChildren<Animator>().SetBool("isAttacking",false);
        Invoke("stopEnemyHitFunc",0.4f);
    }

    private void stopEnemyHitFunc(){
        // enable movement again
        gameObject.GetComponentInChildren<Animator>().SetBool("shot",false);
    }

    public IEnumerator instantiateDamageOverlay(float newAlpha,float time){
        float currentAlpha = damageOverlay.color.a;
        Color currentColor = damageOverlay.color;
        Color newColor = damageOverlay.color;
        newColor.a = newAlpha;
        float t = 0f;
        while(t < time){
            t += Time.deltaTime / time;
            damageOverlay.color = Color.Lerp(currentColor,newColor,t);
            yield return null;
        }
    }
}
