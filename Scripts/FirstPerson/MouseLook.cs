using UnityEngine;
using System.Collections;

/// <summary>
/// Mouse look controller for first person
/// </summary>
public class MouseLook : MonoBehaviour
{
    public float lookSensitivity = 5f;
    public float lookSmoothDamp = .1f;
    public float verticalLimit = 90f;
    public bool inverted = false;
    public Transform player;

    float yRotation;
    float xRotation;
    float currentYRotation;
    float currentXRotation;
    float yRotationV;
    float xRotationV;

	
	// Update is called once per frame
	void Update ()
    {
#if UNITY_EDITOR
        Cursor.visible = true;
#else
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif

        int invert = inverted ? -1 : 1;
        yRotation += Input.GetAxis("LookX") * lookSensitivity;
        xRotation -= invert * Input.GetAxis("LookY") * lookSensitivity;

        xRotation = Mathf.Clamp(xRotation, -verticalLimit, verticalLimit);

        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothDamp);

        transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
        if(player != null)
            player.localRotation = Quaternion.Euler(0, currentYRotation, 0);

    }
}
