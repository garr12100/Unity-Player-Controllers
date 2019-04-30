using UnityEngine;
using System.Collections;

using Helper;

/// <summary>
/// Tthis script is included with the PlayerCharacter script includes. It governs the behavior of this instance's
/// ability to cove the camera around the environment using and FSM.
/// </summary>

public class PlayerCamera : MonoBehaviour {


    #region Public Fields and Properties



    #endregion

    #region Private Fields and Properties

    // Used to position the camera: 
    private Vector3 cameraNormalPos = new Vector3(0f, 1.75f, -2.25f);

    //Used to rotate the camera: 
    [SerializeField]
    private float sensitivity = 5f;
    [SerializeField]
    private float minimumAngle = -40f;
    [SerializeField]
    private float maximumAngle = 60f;

    private float rotationY = 0f;

    private Transform camera;
    private Transform player;

    private CameraState state = CameraState.Normal;

    private CameraTargetObject cameraTargetObject;
    private CameraMountPoint cameraMountPoint;

    #endregion

    #region Getters and Setters

    public CameraState CameraState
    {
        get { return state; }
    }

    #endregion

    #region Methods

    // Use this for initialization
    void Start ()
    {
        //Ensure a networkView exists
        if (GetComponent<NetworkView>() != null)
        {
            //Ensure that initialization only executes if this is a valid instance. 
            if (GetComponent<NetworkView>().isMine || Network.peerType == NetworkPeerType.Disconnected)
            {
                camera = GameObject.FindGameObjectWithTag(GameTag.PlayerCamera).transform;
                player = this.transform;

                //Create an object at runtime for the camera to look at. 
                cameraTargetObject = new CameraTargetObject();
                cameraTargetObject.Init("Camera Target", new Vector3(0f,1f,0f), new GameObject().transform, player.transform);

                // Create an empty object at runtime for the camera to look at. 
                cameraMountPoint = new CameraMountPoint();
                cameraMountPoint.Init("Camera Mount", cameraNormalPos, new GameObject().transform, cameraTargetObject.XForm);

                camera.parent = cameraTargetObject.XForm.parent;
            }
            else
            {
                enabled = false;
            }
        }
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        // FSM used to govern how this camera is supposed to behave.
        switch (state)
        {
            case CameraState.Normal:

                RotateCamera();
                camera.position = cameraMountPoint.XForm.position;
                camera.LookAt(cameraTargetObject.XForm);

                break;
            case CameraState.Target:

                break;
        }
	}

    #endregion

    #region Custom Methods

    private void RotateCamera()
    {
        rotationY += Input.GetAxis(PlayerInput.LookY) * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minimumAngle, maximumAngle);

        cameraTargetObject.XForm.localEulerAngles = new Vector3(-rotationY, cameraTargetObject.XForm.localEulerAngles.y, 0);
    }

    #endregion
}
