using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Behaviour
/// A Specific Behavior to be carried out by AI. 
/// Example: Walk to target, begin attacking, etc. 
/// </summary>
[RequireComponent(typeof(AIController))]
public abstract class AIBehaviour : MonoBehaviour
{
    public float lookSpeed = 5f;

    protected AIController controller;

    protected virtual void Start()
    {
        controller = GetComponent<AIController>();
    }

    protected virtual void Update ()
    {

	}

    

    protected virtual void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }

    

}
