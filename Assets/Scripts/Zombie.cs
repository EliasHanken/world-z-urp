using UnityEngine;
using System.Collections;


public class Zombie : MonoBehaviour
{
    private class BoneTransform
    {
        public Vector3 Position { get; set; }

        public Quaternion Rotation { get; set; }
    }

    public enum ZombieState
    {
        Idle,
        Walking,
        Running,
        Ragdoll,
        StandingUp,
        ResettingBones,
        Dead
    }

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private string _standUpStateName;

    [SerializeField]
    private string _standUpClipName;

    [SerializeField]
    private float _timeToResetBones;

    private Rigidbody[] _ragdollRigidbodies;
    public ZombieState _currentState = ZombieState.Idle;
    private Animator _animator;
    private CharacterController _characterController;
    private float _timeToWakeUp;
    private Transform _hipsBone;

    private BoneTransform[] _standUpBoneTransforms;
    private BoneTransform[] _ragdollBoneTransforms;
    private Transform[] _bones;
    private float _elapsedResetBonesTime;

    public bool _reborn = false;
    public bool canBeReborn = true;
    public bool isDead = false;
    public float eyeSightLength = 5f;
    public Transform eyeSight;

    void Awake()
    {
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _hipsBone = _animator.GetBoneTransform(HumanBodyBones.Hips);

        _bones = _hipsBone.GetComponentsInChildren<Transform>();
        _standUpBoneTransforms = new BoneTransform[_bones.Length];
        _ragdollBoneTransforms = new BoneTransform[_bones.Length];

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            _standUpBoneTransforms[boneIndex] = new BoneTransform();
            _ragdollBoneTransforms[boneIndex] = new BoneTransform();
        }

        PopulateAnimationStartBoneTransforms(_standUpClipName, _standUpBoneTransforms);

        DisableRagdoll();

        StartCoroutine(rotateZombieRandom());
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case ZombieState.Walking:
                WalkingBehaviour();
                break;
            case ZombieState.Running:
                RunningBehaviour();
                break;
            case ZombieState.Ragdoll:
                RagdollBehaviour();
                break;
            case ZombieState.StandingUp:
                StandingUpBehaviour();
                break;
            case ZombieState.ResettingBones:
                ResettingBonesBehaviour();
                break;
            case ZombieState.Dead:
                DeadBehaviour();
                break;
            case ZombieState.Idle:
                _animator.SetBool("run",false);
                _animator.SetBool("idle",true);
                _animator.SetBool("walk",false);
                RaycastHit hit;
                if(Physics.Raycast(eyeSight.position,eyeSight.forward,out hit,eyeSightLength)){
                    if(hit.collider.tag == "Player"){
                        _currentState = ZombieState.Running;
                    }
                }
                break;
            }
        Debug.DrawRay(eyeSight.position,eyeSight.forward * eyeSightLength,Color.red);
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    { 
        EnableRagdoll();

        Rigidbody hitRigidbody = FindHitRigidbody(hitPoint);

        hitRigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);

        _currentState = ZombieState.Ragdoll;
        _timeToWakeUp = Random.Range(5, 10);
    } 


    public Rigidbody FindHitRigidbody(Vector3 hitPoint)
    {
        Rigidbody closestRigidbody = null;
        float closestDistance = 0;

        foreach (var rigidbody in _ragdollRigidbodies)
        {
            float distance = Vector3.Distance(rigidbody.position, hitPoint);

            if (closestRigidbody == null || distance < closestDistance)
            {
                closestDistance = distance;
                closestRigidbody = rigidbody;
            }
        }

        return closestRigidbody;
    }

    private IEnumerator rotateZombieRandom(){
        while(true){
            int wait_time = Random.Range(5,10);
            yield return new WaitForSeconds(wait_time);
            if(_currentState == ZombieState.Idle){
                _animator.SetTrigger("turn");
            }
            
        }
    }
      
    public void DisableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        _characterController.enabled = true;
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        _animator.enabled = false;
        _characterController.enabled = false;
    }

    private void WalkingBehaviour()
    {
        Vector3 direction = _camera.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);
    }

    private void RunningBehaviour()
    {
        bool containsPlayer = false;
        GameObject player = null;
        foreach(Collider collider in Physics.OverlapSphere(transform.position,eyeSightLength)){
            if(collider.tag == "Player"){
                containsPlayer = true;
                player = collider.gameObject;
            }
        }
        if(containsPlayer){
            RaycastHit hit;
            if(Physics.Raycast(eyeSight.position,eyeSight.forward,out hit,eyeSightLength)){
                if(hit.collider.tag != "Player"){
                    _currentState = ZombieState.Idle;
                    return;
                }
            }
            if(Vector3.Distance(player.transform.position,transform.position) <= 2.4f){
                _animator.SetBool("attack",true);
            }else{
                _animator.SetBool("attack",false);
            }
        }else{
            _currentState = ZombieState.Idle;
            return;
        }
        
        Vector3 direction = _camera.transform.position - transform.position;
        direction.y = 0;
        direction.Normalize();

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 260 * Time.deltaTime);
        
        _animator.SetBool("run",true);

        _animator.SetBool("idle",false);
        _animator.SetBool("walk",false);
    }

    private void RagdollBehaviour()
    {
        if(_reborn){
            isDead = true;
            _currentState = ZombieState.Dead;
            return;
        }
        _timeToWakeUp -= Time.deltaTime;

        if (_timeToWakeUp <= 0 && canBeReborn)
        {
            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransforms(_ragdollBoneTransforms);

            _currentState = ZombieState.ResettingBones;
            _elapsedResetBonesTime = 0;

            GetComponent<EntityHealth>().health = 5;
            _reborn = true;
        }
    }

    private void DeadBehaviour()
    {
        EnableRagdoll();
    }

    private void StandingUpBehaviour()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_standUpStateName) == false)
        {
            _currentState = ZombieState.Running;
        }
    }

    private void ResettingBonesBehaviour()
    {
        _elapsedResetBonesTime += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTime / _timeToResetBones;

        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex ++)
        {
            _bones[boneIndex].localPosition = Vector3.Lerp(
                _ragdollBoneTransforms[boneIndex].Position,
                _standUpBoneTransforms[boneIndex].Position,
                elapsedPercentage);

            _bones[boneIndex].localRotation = Quaternion.Lerp(
                _ragdollBoneTransforms[boneIndex].Rotation,
                _standUpBoneTransforms[boneIndex].Rotation,
                elapsedPercentage);
        }

        if (elapsedPercentage >=1)
        {
            _currentState = ZombieState.StandingUp;
            DisableRagdoll();

            _animator.Play(_standUpStateName);
        }
    }

    private void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up * -1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }

    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = _hipsBone.position; 
        transform.position = _hipsBone.position;

        Vector3 positionOffset = _standUpBoneTransforms[0].Position;
        positionOffset.y = 0;
        positionOffset = transform.rotation * positionOffset;
        transform.position -= positionOffset;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }
        
        _hipsBone.position = originalHipsPosition;
    }

    private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
    {
        for (int boneIndex = 0; boneIndex < _bones.Length; boneIndex++)
        {
            boneTransforms[boneIndex].Position = _bones[boneIndex].localPosition;
            boneTransforms[boneIndex].Rotation = _bones[boneIndex].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransforms)
    {
        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;

        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                clip.SampleAnimation(gameObject, 0);
                PopulateBoneTransforms(_standUpBoneTransforms);
                break;
            }
        }

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }
}