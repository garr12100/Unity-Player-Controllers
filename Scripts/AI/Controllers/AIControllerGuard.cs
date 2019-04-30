using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI Controller defining guard behavior = Patrols until player is within range, then attacks. 
/// </summary>
[RequireComponent(typeof(AIPatrol))]
[RequireComponent(typeof(AIVisionHostile))]
public class AIControllerGuard : AIController
{
    public float pauseTime = 1f;


    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        SetBehaviour(typeof(AIPatrol));
	}

    private void Update()
    {
        if (overlappingColliders != null)
        {
            if (playerInSight)
                SwitchToFollow();
            else
                SwitchToPatrol();
        }
    }

    private void SwitchToFollow()
    {
        StopAllCoroutines();
        GetBehaviour(typeof(AIPatrol)).StopAllCoroutines();
        SetBehaviour(typeof(AIVisionHostile));
    }

    private void SwitchToPatrol()
    {
        StartCoroutine(PauseThenPatrol());
    }

    IEnumerator PauseThenPatrol()
    {
        yield return new WaitForSeconds(pauseTime);
        SetBehaviour(typeof(AIPatrol));

    }

}
