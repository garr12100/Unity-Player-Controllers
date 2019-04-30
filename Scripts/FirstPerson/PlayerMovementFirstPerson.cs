using UnityEngine;
using System.Collections;

/// <summary>
/// Movement controller for first person
/// </summary>
public class PlayerMovementFirstPerson : MonoBehaviour
{
    public GameObject cameraObject;
    public float speed = 10.0f;
    public float gravity = 10.0f;
    public float maxVelocityChange = 10.0f;
    public float maxVelocityChangeAirRatio = .3f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public Grounder grounder;
    public bool playerControlled = true;

    private Rigidbody rbody;
    private bool jumpNow = false;

	// Use this for initialization
	void Start ()
    {
        if (grounder == null)
            grounder = GetComponent<Grounder>();
        rbody = GetComponent<Rigidbody>();
        rbody.freezeRotation = true;
        rbody.useGravity = false;
	}

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            jumpNow = true;
    }

    void FixedUpdate()
    {
        ////Rotation: 
        //if (playerControlled)
        //{
        //    transform.rotation = Quaternion.Euler(0, cameraObject.transform.eulerAngles.y, 0);
        //}

        //Walking:
        // Calculate how fast we should be moving
        Vector3 targetVelocity = playerControlled ?  new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) : Vector3.zero;
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        float clampVal = grounder.Grounded ? maxVelocityChange : maxVelocityChange * maxVelocityChangeAirRatio;
        velocityChange.x = Mathf.Clamp(velocityChange.x, -clampVal, clampVal);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -clampVal, clampVal);
        velocityChange.y = 0;
        rbody.AddForce(velocityChange, ForceMode.VelocityChange);

        // Jump
        if (jumpNow)
            Jump();

        // We apply gravity manually for more tuning control
        rbody.AddForce(new Vector3(0, -gravity * rbody.mass, 0));
    }

    void Jump()
    {
        jumpNow = false;
        if (grounder.Grounded)
        {
            rbody.velocity = new Vector3(rbody.velocity.x, CalculateJumpVerticalSpeed(), rbody.velocity.z);
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}
