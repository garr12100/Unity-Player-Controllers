using UnityEngine;
using System.Collections;

public class MatchCamera : MonoBehaviour
{
    public Camera thisCamera;
    public Camera cameraToMatch;

    public bool matchPosition = true;
    public bool matchRotation = true;
	// Use this for initialization
	void Start ()
    {
        if (thisCamera == null)
            thisCamera = gameObject.GetComponent<Camera>();

        if (cameraToMatch == null)
            cameraToMatch = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (matchPosition)
            thisCamera.transform.position = cameraToMatch.transform.position;
        if (matchRotation)
            thisCamera.transform.rotation = cameraToMatch.transform.rotation;

        thisCamera.projectionMatrix = cameraToMatch.projectionMatrix;
        thisCamera.fieldOfView = cameraToMatch.fieldOfView;
        thisCamera.farClipPlane = cameraToMatch.farClipPlane;
        thisCamera.nearClipPlane = cameraToMatch.nearClipPlane;
	}
}
