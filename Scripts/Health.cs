using UnityEngine;
using System.Collections;

using Helper;

public class Health : MonoBehaviour
{
    #region Public Variables
    #endregion

    #region Private Variables
    [SerializeField]
    private float currentHealth = 5;

    #endregion

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (DeathCheck())
            Die();
    }

    protected virtual bool DeathCheck()
    {
        if (currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
