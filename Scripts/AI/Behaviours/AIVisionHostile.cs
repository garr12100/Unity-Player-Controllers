using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior defining hostile enemy - faces Player when within range. 
/// </summary>
public class AIVisionHostile : AIBehaviour
{
    protected override void Update()
    {
        base.Update();
        if (controller.OverlappingCollider == null)
            return;
        if (controller.PlayerInSight)
        {
            controller.Agent.destination = controller.offendingTransforms[0].transform.position;
            if (Vector3.Distance(transform.position, controller.offendingTransforms[0].transform.position) <= controller.Agent.stoppingDistance)
            {
                FaceTarget(controller.offendingTransforms[0].transform);
            }
        }
    }

}
