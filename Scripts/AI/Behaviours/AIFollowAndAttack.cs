using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior defining follow target and stop a certain distance away. 
/// </summary>
public class AIFollowAndAttack : AIBehaviour
{
	protected override void  Update ()
    {
        base.Update();
        if (controller.OverlappingCollider == null)
            return;
        foreach (Collider col in controller.OverlappingCollider)
        {
            //If Player is close, move towards player. 
            if (col.CompareTag("Player"))
            {
                controller.Agent.destination = col.transform.position;
                if (Vector3.Distance(transform.position, col.transform.position) <= controller.Agent.stoppingDistance)
                {
                    FaceTarget(col.transform);
                }
            }
        }
	}

    
}
