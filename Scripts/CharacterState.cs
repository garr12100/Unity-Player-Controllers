using UnityEngine;
using System.Collections;

/// <summary>
/// Holds data about the current state of a player which may be used to prevent actions from overlapping. 
/// </summary>
public class CharacterState : MonoBehaviour
{
    public bool attacking;
    public bool sprinting;
    public bool targeting;

    public void SetAttacking()
    {
        attacking = true;
    }

    public void ClearAttacking()
    {
        attacking = false;

    }
    public void SetSprinting()
    {
        sprinting = true;

    }

    public void ClearSprinting()
    {
        sprinting = false;

    }

    public void SetTargeting()
    {
        targeting = true;

    }

    public void ClearTargeting()
    {
        targeting = false;

    }
}
