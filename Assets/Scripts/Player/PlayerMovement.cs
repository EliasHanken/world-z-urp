using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Breathing breathing;
    public Transform breathingTransform;
    public RectTransform crosshair;
    [Header("Movement")]
    [Space]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float sprintSpeed;
    [SerializeField] public float crouchSpeed;
    [SerializeField] public float ADSSpeed;

    [SerializeField] public float groundDrag;
    [SerializeField] public float airDrag;

    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airMultiplier;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float timeToCrouch = 0.25f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0,0.5f,0);
    [SerializeField] private Vector3 standingCenter = new Vector3(0,0,0);
    private bool duringCrouchAnimation;
    [Space]
    public bool setADS = false;
    public bool canCrouch = true;
    public bool isIdle, isWalking, isRunning, isJumping, isCrouching;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public AudioClip concreteAudioClip;
    public AudioClip concreteRunAudioClip;
    public AudioSource feetAudioSource;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    private Vector3 velocityBeforeJump;
    private bool initializeJump = false;
    private float _playerColliderHeight;
    public Animator anim;
    public Camera cam;

    Vector3 moveDirection;

    Rigidbody rb;

    bool walkCouroutineStarted = false;
    bool runCouroutineStarted = false;
    float currentCrosshairAlpha1;
    float currentCrosshairAlpha2;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        _playerColliderHeight = gameObject.GetComponent<CapsuleCollider>().height;

        StartCoroutine(PlayFootsteps());

        AudioListener.volume = 0.4f; // TODO CHANGE
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();

        if(isCrouching){
            //crouch();
        }



        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            {
                rb.drag = 0;
                Vector3 t_vel = rb.velocity;

                t_vel.x *= airDrag;
                t_vel.z *= airDrag;

                rb.velocity = t_vel;
            }

        anim.SetBool("idle",isIdle);
        if(!grounded){
            isJumping = true;
        }else{
            isJumping = false;
        }

        if(isJumping){
            anim.SetBool("jump",true);
            return;
        }else{
            anim.SetBool("jump",false);
        }

        if(anim.GetBool("walk") && anim.GetBool("run")){
            if(isRunning){
                anim.SetBool("run",true);
                anim.SetBool("walk",false);
            }else if(isWalking){
                anim.SetBool("run",false);
                anim.SetBool("walk",true);
            }
        }

        if(setADS){
            anim.SetBool("ads",true);
            breathing.enabled = false;
            //breathingTransform.position = new Vector3(0,0,0.4572096f);
            
            foreach(Image img in crosshair.GetComponentsInChildren<Image>()){
                currentCrosshairAlpha1 = Mathf.Lerp(img.color.a, 0, Time.deltaTime * 40);

                Color newColor = img.color;
                newColor.a = currentCrosshairAlpha1;
                img.color = newColor;
            }
            return;
        }else if(!setADS){
            anim.SetBool("ads",false);
            breathing.enabled = true;
            foreach(Image img in crosshair.GetComponentsInChildren<Image>()){
                currentCrosshairAlpha2 = Mathf.Lerp(img.color.a, 1, Time.deltaTime * 40);

                Color newColor1 = img.color;
                newColor1.a = currentCrosshairAlpha2;
                img.color = newColor1;
            }
        }

        if(isRunning && anim.GetBool("shoot") || isRunning && anim.GetBool("reload")){
            anim.SetBool("run",false);
            return;
        }else if(isRunning && !anim.GetBool("shoot") || isRunning && !anim.GetBool("reload")){
            anim.SetBool("run",isRunning);
            return;
        }

        if(isWalking && anim.GetBool("shoot") || isWalking && anim.GetBool("reload")){
            anim.SetBool("walk",false);
            return;
        }else if(isWalking && !anim.GetBool("shoot") || isWalking && !anim.GetBool("reload")){
            anim.SetBool("walk",isWalking);
            return;
        }

        if(!isWalking){
            anim.SetBool("run",isRunning);
            anim.SetBool("walk",false);
        }
        if(!isRunning) anim.SetBool("walk",isWalking);

        
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if(grounded){
            if(setADS){
                isIdle = false;
                isWalking = false;
                isRunning = false;
                
                rb.AddForce(moveDirection.normalized * ADSSpeed * 10f, ForceMode.Force);
                return;
            }
            if(moveDirection.x == 0 && moveDirection.y == 0 && moveDirection.z == 0){
                isIdle = true;

                isRunning = false;
                isWalking = false;
                return;
            }
            if(!Input.GetKey(KeyCode.LeftControl)){
                //gameObject.GetComponent<CapsuleCollider>().height = _playerColliderHeight;
                isCrouching = false;
            }
            if(Input.GetKey(KeyCode.LeftControl)){
                rb.AddForce(moveDirection.normalized * crouchSpeed * 10f, ForceMode.Force);
                //gameObject.GetComponent<CapsuleCollider>().height = _playerColliderHeight / 2f;
                isWalking = false;
                isRunning = false;
                isIdle = false;

                isCrouching = true;
            }
            else if(Input.GetKey(KeyCode.LeftShift)){
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force);
                gameObject.GetComponent<CapsuleCollider>().height = _playerColliderHeight;
                isIdle = false;
                isCrouching = false;
                isWalking = false;

                isRunning = true;
            }else if(moveDirection.x == 0 && moveDirection.y == 0 && moveDirection.z == 0){
                isIdle = true;

                isRunning = false;
                isIdle = false;
                isWalking = false;
            }else{
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
                //gameObject.GetComponent<CapsuleCollider>().height = _playerColliderHeight;
                isRunning = false;
                //isCrouching = false;
                isIdle = false;

                isWalking = true;
            }
            
                
        }
            

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        isJumping = true;
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void crouch(){
        StartCoroutine(crouchStand());
    }

    private IEnumerator crouchStand(){
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = gameObject.GetComponent<CapsuleCollider>().height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = gameObject.GetComponent<CapsuleCollider>().center;

        while(timeElapsed < timeToCrouch){
            gameObject.GetComponent<CapsuleCollider>().height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed/timeToCrouch);
            gameObject.GetComponent<CapsuleCollider>().center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed/timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.GetComponent<CapsuleCollider>().height = targetHeight;
        gameObject.GetComponent<CapsuleCollider>().center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private IEnumerator PlayFootsteps(){
        while(true){
            if(grounded){
                if(!isWalking && !isRunning){
                    feetAudioSource.Stop();
                }
                if(!isWalking && !runCouroutineStarted){
                    feetAudioSource.Stop();
                }
                if(!isRunning && !walkCouroutineStarted){
                    feetAudioSource.Stop();
                }
                if(isRunning && !runCouroutineStarted){
                    yield return StartCoroutine(PlayFootstepsRunning());
                }
                if(isWalking && !walkCouroutineStarted){
                    yield return StartCoroutine(PlayFootstepsWalking());
                }
                if(isWalking){
                    yield return StartCoroutine(PlayFootstepsWalking());
                }if(isRunning){
                    yield return StartCoroutine(PlayFootstepsRunning());
                }
            }
            yield return null;
        }
        
    }

    private IEnumerator PlayFootstepsWalking(){
        if(isRunning){
                yield return null;
            }
        if(!walkCouroutineStarted){
            walkCouroutineStarted = true;
            if(isRunning){
                yield return null;
            }
            yield return new WaitForSeconds(0.25f);
        }
        if(isWalking && walkCouroutineStarted){
            if(isRunning){
                yield return null;
            }
            feetAudioSource.PlayOneShot(concreteAudioClip);
            yield return new WaitForSeconds(concreteAudioClip.length);
        }
        if(!isWalking){
            walkCouroutineStarted = false;
            yield return null;
        }
    }

    private IEnumerator PlayFootstepsRunning(){
        if(!runCouroutineStarted){
            runCouroutineStarted = true;
            yield return null;
        }
        if(isRunning && runCouroutineStarted){
            if(isWalking){
                yield return null;
            }
            feetAudioSource.PlayOneShot(concreteRunAudioClip);
            yield return new WaitForSeconds(concreteRunAudioClip.length);
        }
        if(!isRunning){
            runCouroutineStarted = false;
            yield return null;
        }
    }
}