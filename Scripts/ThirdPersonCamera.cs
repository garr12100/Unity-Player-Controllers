using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

using Helper;

public class ThirdPersonCamera : MonoBehaviour
{

    #region Public Variables

    #endregion

    #region Private Variables

    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float distanceOffset;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private float sensitivity;
    [SerializeField]
    private float minimumAngle = -40f;
    [SerializeField]
    private float maximumAngle = 60f;
    [SerializeField]
    private float yOffset = 0.0f;
    [SerializeField]
    private float zOffset = 0.0f;
    [SerializeField]
    private float normalZoom = 0.0f;
    [SerializeField]
    private float normalElevate = 0.0f;
    [SerializeField]
    private float normalOffset = 0.0f;
    [SerializeField]
    private float targetZoom = 0.0f;
    [SerializeField]
    private float targetElevate = 0.0f;
    [SerializeField]
    private float targetOffset = 0.0f;
    [SerializeField]
    private float sprintZoom = 0.0f;
    [SerializeField]
    private float sprintElevate = 0.0f;
    [SerializeField]
    private float sprintOffset = 0.0f;
    [SerializeField]
    private float bumperRadius = 0.0f;
    private CameraTargetObject follow;
    private Vector3 targetPosition;
    private float rotationY;
    private float rotationX;
    private Vector3 targetAngle;
    private CameraState camState = CameraState.Normal;
    private Transform target;
    private bool newLookMode = false;
    private bool hasTarget = false;
    private IEnumerator CameraLookRoutine;
    private GameObject player;
    private GameObject cam;
    

    #endregion

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag(GameTag.Player);
        cam = GameObject.Find("Camera");
        follow = new CameraTargetObject();
        follow.Init("Camera Target", new Vector3(0f, yOffset, zOffset), new GameObject().transform, player.transform);
        PlayerMotor pm = player.GetComponent<PlayerMotor>();
        pm.OnNormalMode += NormalMode;
        pm.OnTargetMode += TargetMode;
        pm.OnAcquireTarget += NewTarget;
        pm.OnSprintMode += SprintMode;
        //Hide cursor if build
#if UNITY_EDITOR
        Cursor.visible = true;
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif



    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Set the target position to be correct offset from camera "hovercraft"
        targetPosition = follow.XForm.position + (follow.XForm.up * distanceUp) - (follow.XForm.forward * distanceAway) + (follow.XForm.right * distanceOffset);


        //Make sure the camera is looking the right direction: 
        switch (camState)
        {
            case CameraState.Normal:

                //Rotate camera up/down:
                RotateCamera();

                if (!newLookMode)
                {
                    transform.LookAt(follow.XForm);

                }
                break;

            case CameraState.Target:

                if (target != null)
                {
                    if (!newLookMode)
                    {
                        transform.LookAt(target);
                    }
                }
                else
                {
                    if (hasTarget)
                    {
                        NewTarget(null);
                        hasTarget = false;
                    }
                    if (!newLookMode)
                    {
                        transform.LookAt(follow.XForm);
                    }

                }
                break;

            case CameraState.Sprint:

                if (!newLookMode)
                {
                    transform.LookAt(follow.XForm);

                }
                break;
        }
        CompensateForWalls();

        //Make a smooth transition between its current position and the position it wants to be in
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

    }

    void OnDestroy()
    {
        //Remove all event subscriptions
        if (player != null)
        {
            PlayerMotor pm = player.GetComponent<PlayerMotor>();
            pm.OnNormalMode -= NormalMode;
            pm.OnTargetMode -= TargetMode;
            pm.OnAcquireTarget -= NewTarget;
            pm.OnSprintMode -= SprintMode;
        }
    }


    IEnumerator CameraLook(Transform pos)
    {
        //Smoothly point the camera at a target when a new one has been found.
        float currentTime = 0.0f;
        float duration = 1f;
        while (currentTime <= duration)
        {
            currentTime += Time.deltaTime;
            if (pos != null)
            {
                Vector3 lookDir = pos.position - transform.position;
                Quaternion newRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, currentTime / duration);
            }
            yield return 0;
        }
        newLookMode = false;

    }

    private void RotateCamera()
    {
        //Rotates camera up and down based on mouse/joystick movement. 
        rotationY += Input.GetAxis(PlayerInput.LookY) * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumAngle, maximumAngle);
        follow.XForm.localEulerAngles = new Vector3(-rotationY, follow.XForm.localEulerAngles.y, 0);
    }

    private void CompensateForWalls()
    {
        //Compensate for walls between camera and player
        RaycastHit wallHit = new RaycastHit();
        Debug.DrawLine(player.transform.position, targetPosition);
        if (Physics.Linecast(player.transform.position, targetPosition, out wallHit, ~(1<<GameLayers.Player)))
        {
            targetPosition = wallHit.point + (follow.XForm.position - targetPosition).normalized * bumperRadius;
        }
    }

    public void TargetMode(Transform trgt)
    {
        //Enter Target Mode
        camState = CameraState.Target;
        distanceUp = targetElevate;
        distanceAway = targetZoom;
        distanceOffset = targetOffset;
        follow.XForm.localEulerAngles = Vector3.zero;
        NewTarget(trgt);
    }

    public void NewTarget(Transform trgt)
    {
        //Acquire new target and handle coroutines for looking at it
        target = trgt;
        hasTarget = true;
        if (newLookMode)
        {
            StopCoroutine(CameraLookRoutine);
        }
        newLookMode = true;
        if (target != null)
        {
            CameraLookRoutine = CameraLook(target);
        }
        else
        {
            CameraLookRoutine = CameraLook(follow.XForm);
        }
        StartCoroutine(CameraLookRoutine);

    }

    public void NormalMode()
    {
        //Enter Normal Mode
        camState = CameraState.Normal;
        distanceUp = normalElevate;
        distanceAway = normalZoom;
        distanceOffset = normalOffset;
        cam.GetComponent<CameraMotionBlur>().enabled = false;
        if (newLookMode)
        {
            StopCoroutine(CameraLookRoutine);
        }
        newLookMode = true;
        CameraLookRoutine = CameraLook(follow.XForm);
        StartCoroutine(CameraLookRoutine);

    }

    public void SprintMode()
    {
        //Enter Normal Mode
        camState = CameraState.Sprint;
        distanceUp = sprintElevate;
        distanceAway = sprintZoom;
        distanceOffset = sprintOffset;
        cam.GetComponent<CameraMotionBlur>().enabled = true;
        follow.XForm.localEulerAngles = Vector3.zero;
        if (newLookMode)
        {
            StopCoroutine(CameraLookRoutine);
        }
        newLookMode = true;
        CameraLookRoutine = CameraLook(follow.XForm);
        StartCoroutine(CameraLookRoutine);

    }


}
