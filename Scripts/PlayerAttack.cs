using UnityEngine;
using System.Collections;
using System;

using Helper;

public class PlayerAttack : MonoBehaviour
{

    public GameObject attackBox;
    public event Action OnShakeCamera;

    private Animator animator;
    private bool comboHold1;
    private bool comboHold2;
    private bool attackHold;
    private CharacterState cState;


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        cState = GetComponent<CharacterState>();
        comboHold1 = false;
        comboHold2 = false;
        attackHold = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Attack1:
        if (Input.GetButtonDown(PlayerInput.Attack1) && !attackHold && !cState.sprinting)
        {
            Attack1();
        }
        if (Input.GetButtonDown(PlayerInput.Attack2) && !attackHold && !cState.sprinting)
        {
            Attack2();
        }
    }

    private void Attack1()
    {
        if (!comboHold1)
        {
            cState.SetAttacking();
            animator.SetTrigger(AnimatorConditionsPlayer.Attack1);
            comboHold1 = true;
            comboHold2 = true;
        }
        else
        {
            //Player attacked too soon, end combo
            //attackHold = true;
        }
    }

    private void Attack2()
    {
        if (!comboHold2)
        {
            cState.SetAttacking();
            animator.SetTrigger(AnimatorConditionsPlayer.Attack2);
            comboHold2 = true;
            comboHold1 = true;
        }
        else
        {
            //Player attacked too soon, end combo
            //attackHold = true;
        }
    }

    public void SpawnAttackBox(int damage)
    {
        Vector3 spawnPoint = transform.position + transform.forward + transform.up * 1.2f;
        GameObject aBox = Instantiate(attackBox, spawnPoint, Quaternion.identity) as GameObject;
        aBox.GetComponent<AttackBox>().SetDamage(damage);
    }

    public void ReleaseComboHold1()
    {
        comboHold1 = false;
    }

    public void ReleaseComboHold2()
    {
        comboHold2 = false;

    }

    public void ReleaseAttackHold()
    {
        attackHold = false;
        cState.ClearAttacking();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0.3f;
    }
    public void UnfreezeGame()
    {
        Time.timeScale = 1;
    }
    public void ShakeCamera()
    {
        if (OnShakeCamera != null)
        {
            OnShakeCamera();
        }
    }
}
