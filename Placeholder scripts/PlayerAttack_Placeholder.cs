using UnityEngine;
using System.Collections;

public class PlayerAttack_Placeholder : MonoBehaviour
{
    public Animator playerHandsAnim;
    public EquippedItems equippedItems;
    public float animSpeed = 1f;
    private GameObject weapon;
    private Collider weaponCol;
	
	// Update is called once per frame
	void Update ()
    {
        SetWeapon();
        if (weapon == null || weaponCol == null)
            return;
        if (Input.GetButtonDown("Attack1"))
        {
            playerHandsAnim.speed = animSpeed;
            playerHandsAnim.SetTrigger("Swing1");
        }            
	}

    public void SetWeapon()
    {
        if (equippedItems.weaponGO != null && equippedItems.weaponGO != weapon)
        {
            weapon = equippedItems.weaponGO;
            if (weapon != null)
                weaponCol = weapon.GetComponentInChildren<Collider>();
            else
                weaponCol = null;
        }
    }

    public void EnableWeaponCollider()
    {
        weaponCol.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        weaponCol.enabled = false;
    }
}
