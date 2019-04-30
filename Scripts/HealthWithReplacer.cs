using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWithReplacer : Health
{
    public GameObject deathReplacement;
    public override void Die()
    {
        if (deathReplacement != null)
            Instantiate(deathReplacement, transform.position, transform.rotation);
        base.Die();
    }

}
