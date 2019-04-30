using UnityEngine;
using System.Collections;

using Helper;

/// <summary>
///  This script is attached to the player and includes all dependencies that are required in order for the
/// character controller system to function. No scripts included in this instance should ever be absent.
/// 
/// INCLUDED: 
/// 
/// PlayerCamera.cs - Governs the behavior of the camera and is only active on this instance. 
/// PlayerMotor.cs - Governs the behavior of the player's movement and animation. 
/// 
/// </summary>


[RequireComponent(typeof(NetworkView))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(PlayerCamera))]

[AddComponentMenu("APP/PlayerCharacter")]
public class PlayerCharacter : MonoBehaviour
{

    #region Public Fields and Properties

    #endregion

    #region Private Fields and Properties

    private CharacterController controller;
    private Animator animator;
    private RuntimeAnimatorController animatorController;

    #endregion

    #region Getters and Setters
    /// <summary>
    /// Gets the Animator component.
    /// </summary>
    /// <value>
    /// The animator.
    /// </value>
    public Animator Animator
    {
        get { return this.animator; }
    }

    /// <summary>
    /// Gets the CharacterController component.
    /// </summary>
    /// <value>
    /// The controller.
    /// </value>
    public CharacterController Controller
    {
        get
        { return this.controller; }
    }

    #endregion

    #region System Methods

    void Awake()
    {
        animator = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
    }

    // Use this for initialization
    void Start ()
    {
        //Ensure a networkView exists
        if (GetComponent<NetworkView>() != null)
        {
            //Ensure that initialization only executes if this is a valid instance. 
            if (GetComponent<NetworkView>().isMine || Network.peerType == NetworkPeerType.Disconnected)
            {
                //Load the animator controller at runtime.
                animatorController = Resources.Load(Resource.AnimatorControllerPlayer) as RuntimeAnimatorController;
                animator.runtimeAnimatorController = animatorController;

                controller.center = new Vector3(0f, 1f, 0f);
                controller.height = 1.8f;
            }
            else
            {
                enabled = false;
            }
        }
    }
	

    #endregion
}
