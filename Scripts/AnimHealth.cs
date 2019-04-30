using UnityEngine;
using System.Collections;

using Helper;


/// <summary>
/// Health for animated object
/// </summary>
public class AnimHealth : MonoBehaviour
{
    #region Public Variables
    public GameObject spark;
    #endregion

    #region Private Variables
    [SerializeField]
    private float currentHealth = 5;

    private Animator animator;
    #endregion

    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
	}

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Instantiate(spark, transform.position + transform.up, Quaternion.identity);
        if (!DeathCheck())
        {
            animator.SetTrigger(AnimatorConditionsPlayer.Hit);
        }
    }

    private bool DeathCheck()
    {
        if (currentHealth <= 0)
        {
            CharacterController cc = GetComponent<CharacterController>();
            cc.enabled = false;
            animator.SetTrigger(AnimatorConditionsPlayer.Dead);
            return true;
        }
        return false;
    }
}
