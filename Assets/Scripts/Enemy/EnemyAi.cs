using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("Enemy AI Settings")]
    [SerializeField]
    public GameObject objectToChase;
    public EntityHealth entityHealth;
    public Animator animator;
    public string enemyName = "Zombie";
    public float attackDelay = 1f;
    public float attackDamage = 1f;
    public float lookRadius = 5f;
    private float _delay;
    private bool _attackTriggered = false;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start(){
        animator = GetComponentInChildren<Animator>();
        _delay = attackDelay;
    }
    void Update()
    {
        if(entityHealth.health <= 0){
            return;
        }
        float distance = Vector3.Distance(objectToChase.transform.position, transform.position);
        float stoppingDistance = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance;

        if(animator.GetBool("shot")){
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(transform.position);
            return;
        }

        if(distance > lookRadius){
            animator.SetFloat("Speed",0.0f,0.3f,Time.deltaTime);
            animator.SetBool("hasStopped",true);
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(transform.position);
        }

        if(distance <= lookRadius && distance > stoppingDistance){
            if(!entityHealth.hasDied){
                animator.SetFloat("Speed",1f, 0.2f, Time.deltaTime);
                gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(objectToChase.transform.position);
            }
        }
        
        if(distance <= stoppingDistance){
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(objectToChase.transform.position);
            RotateToTarget();

            animator.SetFloat("Speed",0f,0.3f,Time.deltaTime);
            animator.SetBool("hasStopped",true);
            animator.SetBool("isAttacking",true);

            if(animator.GetBool("isAttacking") && !_attackTriggered){
                //_attackTriggered = true;
                //Invoke("AlignWithAnim",1.1f);
            }
        }
        if(distance > stoppingDistance && distance < lookRadius){
            gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(objectToChase.transform.position);
            animator.SetFloat("Speed",1f, 0.3f, Time.deltaTime);
            animator.SetBool("isAttacking",false);
            animator.SetBool("hasStopped",false);
        }
    }

    private void AlignWithAnim(){
        EntityHealth entityHealth = objectToChase.GetComponent<EntityHealth>();
        if(entityHealth != null){
            entityHealth.TakeDamage(attackDamage,false);
        }
        InvokeRepeating("DealDamage",0f,2.633f);
    }
    private void DealDamage(){
        if(Vector3.Distance(objectToChase.transform.position, transform.position) >= gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().stoppingDistance){
            _attackTriggered = false;
            CancelInvoke();
        }else{
            EntityHealth entityHealth = objectToChase.GetComponent<EntityHealth>();
            if(entityHealth != null){
                //entityHealth.TakeDamage(attackDamage);
            }
        }
        
    }

/*
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    */

    private void RotateToTarget(){

        Vector3 direction = (objectToChase.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        
    }
}
