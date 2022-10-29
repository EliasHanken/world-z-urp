using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMechanics : MonoBehaviour
{
    [Header("Player Settings")]
    public float playerSpeedInGround;
    public float playerSpeedInSprint, playerSpeedInCrouch, playerSpeedInAir, gravityMultiplyer;
    public float jumpForce, gravityMultiplier;
    public CapsuleCollider playerCollider;
    public Animator anim;

    float _xMovement, _yMovement, _playerColliderHeight,_currentSpeed;
    Rigidbody playerRB;
    Vector3 to;
    bool isGrounded, jump, isCrouched, runAnim;
    float distToGround;
    void Start()
    {
        _currentSpeed = playerSpeedInGround;
        _playerColliderHeight = playerCollider.height;
        playerRB = gameObject.GetComponent<Rigidbody>();
        distToGround = gameObject.GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded(){
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    void Update()
    {
        isGrounded = IsGrounded();
        //Physics.gravity = new Vector3(0, -9.8f, 0) * gravityMultiplier;
        
        _xMovement = Input.GetAxisRaw("Horizontal");
        _yMovement = Input.GetAxisRaw("Vertical");
        to = (transform.forward* _yMovement + transform.right* _xMovement).normalized;

        // jump
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump = true;
        }
        crouch();
        sprint();
        float velo = playerRB.velocity.magnitude;
        anim.SetFloat("Velocity", velo);
        anim.SetBool("Run", runAnim);
        if(Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Collect");
        }
    }

    private void FixedUpdate()
    {
        to.y = playerRB.velocity.y*gravityMultiplyer;
        playerRB.velocity = new Vector3(to.x * _currentSpeed, playerRB.velocity.y, to.z * _currentSpeed);
        if(jump == true){
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Force);
            jump = false;
        }
        if(!isGrounded){
            _currentSpeed = playerSpeedInAir;
        }
        if(isCrouched == true)
        {
            playerRB.AddForce(Vector3.down * 30);
        }
    }

    void sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed = playerSpeedInSprint;
            runAnim = true;
        }
        else
        {
            _currentSpeed = playerSpeedInGround;
            runAnim = false;
        }
    }

    void crouch()
    {
        if(Input.GetKey(KeyCode.LeftControl))
        {
            playerCollider.height = _playerColliderHeight / 2f;
            isCrouched = true;
            _currentSpeed = playerSpeedInCrouch;
        }
        else
        {
            playerCollider.height = _playerColliderHeight;
            _currentSpeed = playerSpeedInGround;
            isCrouched = false;
        }
    }
}
