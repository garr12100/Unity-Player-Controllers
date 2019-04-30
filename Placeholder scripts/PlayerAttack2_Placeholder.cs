using UnityEngine;
using System.Collections;

public class PlayerAttack2_Placeholder : MonoBehaviour
{
    public Animator playerHandsAnim;
    public EquippedItems equippedItems;
    public float distance = 2f;
    public float forceAmount = 300f;
    public GameObject impactParticle;

    public bool rayCasting = false;
	
	// Update is called once per frame
	void Update ()
    {
        if (equippedItems.weaponGO != null && Input.GetButtonDown("Attack1"))
        {
            playerHandsAnim.SetTrigger("Swing1");
        }
        if (rayCasting)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
            {
                rayCasting = false;
                Collider col = hit.collider;
                //Apply force to the other object:
                if (col.attachedRigidbody != null)
                {
                    col.attachedRigidbody.AddForce(transform.forward * forceAmount);
                }
                //Create particle for impact: 
                GameObject.Instantiate(impactParticle, hit.point, Quaternion.LookRotation(hit.normal), col.transform);
            }
        }
    }



    public void EnableWeaponCollider()
    {
        rayCasting = true;
    }

    public void DisableWeaponCollider()
    {
        rayCasting = false;
    }
}
