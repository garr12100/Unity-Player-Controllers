using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// AI Controller defines an AI's senses and combines AIBehaviours and a state machine to create an AI actor
/// </summary>
public abstract class AIController : MonoBehaviour
{
    public float lookRadius = 10f;
    public float fieldOfView = 120f;

    public LayerMask lookLayers;

    protected Collider[] overlappingColliders;
    protected NavMeshAgent agent;
    protected AIBehaviour[] behaviours;
    public List<Transform> offendingTransforms = new List<Transform>(); 

    protected bool playerInRange;
    protected bool playerInSight;

    public bool PlayerInRange
    {
        get { return playerInRange; }
    }

    public bool PlayerInSight
    {
        get { return playerInSight; }
    }

    public Collider[] OverlappingCollider
    {
        get { return overlappingColliders; }
    }

    public NavMeshAgent Agent
    {
        get { return agent; }
    }



    protected void SetBehaviour(System.Type type)
    {
        bool hasBehaviour = false;
        foreach (AIBehaviour behaviour in behaviours)
        {
            if (behaviour.GetType() == type)
            {
                hasBehaviour = true;
                break;
            }
        }
        if(hasBehaviour)
            foreach (AIBehaviour behaviour in behaviours)
            {
                if (behaviour.GetType() == type)
                {
                    behaviour.enabled = true;
                }
                else
                    behaviour.enabled = false;
            }
    }

    protected AIBehaviour GetBehaviour(System.Type type)
    {
        foreach (AIBehaviour behaviour in behaviours)
        {
            if (behaviour.GetType() == type)
            {
                return behaviour;
            }
        }
        return null;
    }

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        behaviours = GetComponents<AIBehaviour>();
    }

    protected virtual void FixedUpdate()
    {
        overlappingColliders = Physics.OverlapSphere(transform.position, lookRadius, lookLayers);
        playerInRange = false;
        playerInSight = false;
        offendingTransforms.Clear();
        foreach (Collider col in overlappingColliders)
        {
            //If Player is close, move towards player. 
            if (col.CompareTag("Player"))
            {
                offendingTransforms.Add(col.transform);
                playerInRange = true;
                Vector3 playerDir = (col.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(playerDir, transform.forward);
                if (angle <= fieldOfView / 2f)
                {
                    playerInSight = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.cyan;
        Vector3 rightPoint = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * lookRadius;
        Vector3 leftPoint = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * lookRadius;

        Vector3 forwardPoint = transform.forward * lookRadius;

        Gizmos.DrawRay(transform.position, rightPoint);
        Gizmos.DrawRay(transform.position, leftPoint);
        Gizmos.DrawRay(transform.position, forwardPoint);

    }
}
