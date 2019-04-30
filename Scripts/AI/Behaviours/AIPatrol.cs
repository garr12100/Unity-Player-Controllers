using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior defining Patrol between waypoints, with or without wait time at each.  
/// </summary>
public class AIPatrol : AIBehaviour
{
    [System.Serializable]
    public struct Waypoint
    {
        public Transform position;
        public bool overrideWaitTime;
        public float waitTime;
    }

    public float waitTime;
    public Waypoint[] waypoints;

    int currentWaypoint = 0;
    bool moving = true;

    protected override void Update()
    {
        base.Update();
        if (!moving)
            return;
        controller.Agent.SetDestination(waypoints[currentWaypoint].position.position);
        float distance = Vector3.Distance(transform.position, waypoints[currentWaypoint].position.position);
        if (distance <= controller.Agent.stoppingDistance)
        {
            //Made it to a waypoint, wait, then go to next one: 
            StartCoroutine(WaitAtWayPoint());
        }
    }

    protected void OnEnable()
    {
        moving = true;
    }

    IEnumerator WaitAtWayPoint()
    {
        moving = false;
        float thisWaitTime = waypoints[currentWaypoint].overrideWaitTime ? waypoints[currentWaypoint].waitTime : waitTime;
        yield return new WaitForSeconds(thisWaitTime);
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        moving = true;
    }

}
