using UnityEngine;
using System.Collections;
using System;

using Helper;

/// <summary>
/// This script is included with the PlayerCharacter script includes. It governs the behavior of this instance's ability to move around the
/// environment and animate its model based on the human input. 
/// </summary>

public class PlayerMotor : MonoBehaviour {

    #region Public Fields and Properties

    public float targetSpeed = 2f;
    public float walkSpeed = 3f;
    public float sprintSpeed = 5f;
    public float rotationSpeed = 100f;
    public float jumpHeight = 10f;
    public float gravity = 20f;
    public float targetLookSpeed;
    public float detectOffset;
    public float detectRadius;
    public float sprintTime;
    public Transform target;
    public event Action<Transform> OnTargetMode;
    public event Action OnNormalMode;
    public event Action OnSprintMode;
    public event Action<Transform> OnAcquireTarget;

    #endregion

    #region Private Fields and Properties

    private float horizontal;
    private float vertical;

    private float moveSpeed;
    private float airVelocity = 0f;

    private Transform myTransform;

    private Vector3 moveDirection = Vector3.zero;

    public CharacterController controller;

    public CharacterState cState;

    private Animator animator;

    private SpeedState speedState = SpeedState.Walk;
    private CameraState cameraState = CameraState.Normal;

    private IEnumerator SprintRoutine;

    #endregion

    #region Getters and Setters

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { MoveSpeed = value; }
    }

    #endregion

    #region Methods

    // Use this for initialization
    void Start()
    {

        // Cache references to the child components of this game object. 
        myTransform = this.GetComponent<Transform>();

        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();

        animator.SetBool(AnimatorConditionsPlayer.grounded, true);
        cState = GetComponent<CharacterState>();



    }

    // Update is called once per frame
    void Update()
    {

        
        

        //Initialize movement:
        moveDirection = new Vector3(0f, moveDirection.y, 0f);

        //Move:
        Locomotion();


        //Apply simulated gravity to the character:
        moveDirection.y -= gravity * Time.deltaTime;
        //Move character with that + input: 
        controller.Move(moveDirection * Time.deltaTime);


    }

    #endregion

    #region Custom Methods

    public void Locomotion()
    {
        CalculateSpeed();
        //Read Axes
        horizontal = Input.GetAxis(PlayerInput.Horizontal);
        vertical = Input.GetAxis(PlayerInput.Vertical);

        animator.SetFloat(AnimatorConditionsPlayer.AbsSpeed, Mathf.Abs(vertical));
        animator.SetFloat(AnimatorConditionsPlayer.AbsDirection, Mathf.Abs(horizontal));


        // FSM used to govern how movement is supposed to behave.
        switch (cameraState)
        {

            case CameraState.Normal:

                //Allow player to rotate their character

                if (Mathf.Abs(Input.GetAxis(PlayerInput.LookX)) > 0.1f)
                {
                    myTransform.Rotate(0f, Input.GetAxis(PlayerInput.LookX) * rotationSpeed * Time.deltaTime, 0f);
                }

                if (controller.isGrounded)
                {
                    moveDirection = Vector3.zero;
                    airVelocity = 0f;
                    animator.SetBool(AnimatorConditionsPlayer.grounded, true);
                    // Set the cached input values as the conditions for the animator FSM
                    if (horizontal == 0 && vertical == 0)
                    {
                        //Not moving, just rotating
                        animator.SetFloat(AnimatorConditionsPlayer.direction, Input.GetAxis(PlayerInput.LookX));
                        animator.SetFloat(AnimatorConditionsPlayer.speed, 0f);

                    }
                    else
                    {
                        //Moving
                        animator.SetFloat(AnimatorConditionsPlayer.direction, horizontal);
                        animator.SetFloat(AnimatorConditionsPlayer.speed, vertical);
                    }

                }
                else
                {
                    animator.SetBool(AnimatorConditionsPlayer.grounded, false);
                }
                //Check if we are in an animation which disables movement: 
                if (!cState.attacking)
                {
                    if (Mathf.Abs(vertical) >= .9 && Mathf.Abs(horizontal) >= .9)
                    {
                        //Cut diagonal speed if fully moving diagonally
                        vertical = vertical / 1.414f;
                        horizontal = horizontal / 1.414f;
                    }
                    //Determine direction to move the player.
                    if (Mathf.Abs(horizontal) > .1)
                    {
                        moveDirection.x = horizontal * moveSpeed;
                    }
                    if (Mathf.Abs(vertical) > .1)
                    {
                        //Normal speed
                        moveDirection.z = vertical * moveSpeed;
                    }
                    
                    moveDirection = myTransform.TransformDirection(moveDirection);
                }

                //Input for Going to Target Mode: 
                if (Input.GetButtonDown(PlayerInput.Target))
                {
                    TargetMode();
                }

                //Input for Going to Sprint Mode: 
                if (Input.GetButtonDown(PlayerInput.Sprint) && vertical > .3 && Mathf.Abs(horizontal) < .1 && controller.isGrounded && !cState.attacking)
                {
                    SprintMode();
                }

                break;

            case CameraState.Target:
                //Targeting, player can't rotate, always faces enemy
                if (controller.isGrounded)
                {
                    moveDirection = Vector3.zero;
                    airVelocity = 0f;
                    animator.SetBool(AnimatorConditionsPlayer.grounded, true);
                    // Set the cached input values as the conditions for the animator FSM
                    animator.SetFloat(AnimatorConditionsPlayer.direction, horizontal);
                    animator.SetFloat(AnimatorConditionsPlayer.speed, vertical);
                }
                else
                {
                    animator.SetBool(AnimatorConditionsPlayer.grounded, false);
                }
                //Check if we are in an animation which disables movement: 
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    if (Mathf.Abs(vertical) >= .9 && Mathf.Abs(horizontal) >= .9)
                    {
                        //Cut diagonal speed if fully moving diagonally
                        vertical = vertical / 1.414f;
                        horizontal = horizontal / 1.414f;
                    }
                    //Determine direction to move the player.
                    if (Mathf.Abs(horizontal) > .1)
                    {
                        moveDirection.x = horizontal * moveSpeed;
                    }
                    if (Mathf.Abs(vertical) > .1)
                    {
                        //Normal speed
                        moveDirection.z = vertical * moveSpeed;
                    }
                    if (target != null)
                    {
                        Vector3 lookDir = new Vector3(target.position.x - transform.position.x, 0, target.position.z - transform.position.z);
                        Quaternion newRotation = Quaternion.LookRotation(lookDir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, targetLookSpeed * Time.deltaTime);
                        TargetDistanceCheck();
                    }
                    else
                    {
                        SearchForTarget();
                    }

                    moveDirection = myTransform.TransformDirection(moveDirection);
                }

                // Switch to Normal Mode
                if (Input.GetButtonUp(PlayerInput.Target))
                {
                    NormalMode();
                }

                break;

            case CameraState.Sprint:

                if (controller.isGrounded)
                {
                    moveDirection = Vector3.zero;
                    airVelocity = 0f;
                    animator.SetBool(AnimatorConditionsPlayer.grounded, true);
                    // Set the cached input values as the conditions for the animator FSM

                    animator.SetFloat(AnimatorConditionsPlayer.direction, horizontal);
                    animator.SetFloat(AnimatorConditionsPlayer.speed, vertical);
                }
                else
                {
                    animator.SetBool(AnimatorConditionsPlayer.grounded, false);
                }
                //Check if we are in an animation which disables movement: 
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                {
                    if (Mathf.Abs(vertical) > .3)
                    {
                        //Normal speed
                        moveDirection.z = moveSpeed;
                    }

                    moveDirection = myTransform.TransformDirection(moveDirection);
                }

                if (Input.GetButtonUp(PlayerInput.Sprint) || vertical < .3)
                {
                    NormalMode();
                }

                break;
        }

        

        animator.SetFloat(AnimatorConditionsPlayer.airSpeed, airVelocity);
        
    }

    private void CalculateSpeed()
    {
        // FSM used to govern how this camera is supposed to behave.
        switch (speedState)
        {
            case SpeedState.Walk:

                moveSpeed = walkSpeed;

                break;
            case SpeedState.Sprint:

                moveSpeed = sprintSpeed;

                break;
            case SpeedState.Target:
                moveSpeed = targetSpeed;

                break;
        }
    }

    private void Jump()
    {
        moveDirection.y = jumpHeight;
        airVelocity += Time.deltaTime;
    }

    private void TargetMode()
    {
        cameraState = CameraState.Target;
        speedState = SpeedState.Target;
        animator.SetBool(AnimatorConditionsPlayer.Running, false);
        if (OnTargetMode != null)
        {
            OnTargetMode(target);
        }
    }

    private void NormalMode()
    {
        cState.ClearSprinting();
        cameraState = CameraState.Normal;
        speedState = SpeedState.Walk;
        animator.SetBool(AnimatorConditionsPlayer.Running, false);
        target = null;
        if (OnNormalMode != null)
        {
            OnNormalMode();
        }
    }

    private void SprintMode()
    {
        cState.SetSprinting();
        cameraState = CameraState.Sprint;
        speedState = SpeedState.Sprint;
        animator.SetBool(AnimatorConditionsPlayer.Running, true);
        target = null;
        if (SprintRoutine != null)
        {
            StopCoroutine(SprintRoutine);        
        }
        SprintRoutine = Sprint();
        StartCoroutine(SprintRoutine);
        if (OnSprintMode != null)
        {
            OnSprintMode();
        }
    }

    private void SearchForTarget()
    {
        float closestDist = Mathf.Infinity;
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward * detectOffset + transform.up, detectRadius);
        foreach (Collider col in cols)
        {
            if (col != null && col.CompareTag(GameTag.Enemy))
            {
                float dist = (transform.position - col.transform.position).sqrMagnitude;
                if (dist < closestDist && (col.transform.position - transform.position).sqrMagnitude < (detectOffset + detectRadius) * (detectRadius + detectRadius))
                {
                    target = col.transform;
                    closestDist = dist;
                }
            }
        }

        if (target != null)
        {
            AcquireTarget();
        }
    }

    private void TargetDistanceCheck()
    {
        if ((target.position - transform.position).sqrMagnitude > (detectOffset + detectRadius) * (detectRadius + detectRadius))
        {
            target = null;
            AcquireTarget();   
        }
    }

    private void AcquireTarget()
    {
        if (OnAcquireTarget != null)
        {
            OnAcquireTarget(target);
        }
    }



    IEnumerator Sprint()
    {
        yield return new WaitForSeconds(sprintTime);
        NormalMode();
    }


    #endregion
}
