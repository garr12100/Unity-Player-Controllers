using UnityEngine;
using System.Collections;

/// <summary>
/// Object used to determine if collider is grounded. 
/// </summary>
public class Grounder : MonoBehaviour
{
    public bool Grounded
    {
        get { return grounded; }
    }
    public float maxSlope = 60f;

    private bool grounded;

    void OnCollisionStay(Collision col)
    {
        foreach (ContactPoint contact in col.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < maxSlope)
                grounded = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        grounded = false;
    }
}
