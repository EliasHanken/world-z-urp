using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private enum ZombieState
    {
        Walking,
        Ragdoll
    }
    [SerializeField]
    private Camera _camera;
    private Rigidbody[] _ragdollRigidbodies;
    private ZombieState _currentState = ZombieState.Walking;
    private Animator _animator;
    private CharacterController _characterController;
    void Awake(){
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        DisableRagdoll();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case ZombieState.Walking:
                WalkingBehaviour();
                break;
            case ZombieState.Ragdoll:
                RagdollBehaviour();
                break;
        }
    }

    public void TriggerRagdoll(Vector3 force, Vector3 hitPoint)
    {
        EnableRagdoll();

        Rigidbody hitRigidbody = FindHitRigidbody(hitPoint);
        // Link, not performance optimized?
        //_ragdollRigidbodies.OrderBy(rigidbody => Vector3.Distance(rigidbody.position, hitPoint)).First();

        hitRigidbody.AddForceAtPosition(force, hitPoint,ForceMode.Impulse);

        _currentState = ZombieState.Ragdoll;
    }

    private Rigidbody FindHitRigidbody(Vector3 hitPoint)
    {
        Rigidbody closestRigidbody = null;
        float closestDistance = 0;

        foreach(var rigidbody in _ragdollRigidbodies)
        {
            float distance = Vector3.Distance(rigidbody.position, hitPoint);

            if(closestRigidbody == null || distance < closestDistance)
            {
                closestDistance = distance;
                closestRigidbody = rigidbody;
            }
        }

        return closestRigidbody;
    }

    private void DisableRagdoll(){
        foreach (var rigidbody in _ragdollRigidbodies){
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        _characterController.enabled = true;
    }

    private void EnableRagdoll(){
        foreach (var rigidbody in _ragdollRigidbodies){
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 180 * Time.deltaTime);
    }

    private void RagdollBehaviour()
    {

    }
}
