using UnityEngine;
using System.Collections;

public class WeaponDamagePhysics_Placeholder : MonoBehaviour
{
    public Weapon weapon;
    public GameObject impactParticle;

    void DealDamage()
    {

    }

    float CalculateDamage()
    {
        float damage = weapon.damage + Random.Range(-weapon.damageRange, weapon.damageRange);
        float criticalRoll = Random.Range(0f, 1f);
        if (weapon.criticalChance >= criticalRoll)
        {
            damage += weapon.criticalBoost;
            Debug.Log("Critical Hit!");
        }
        return damage;
    }

    private void OnCollisionEnter(Collision col)
    {
        float damage = CalculateDamage();
        //Hit something!
        Debug.Log(transform.parent.name + " hit " + col.gameObject.name + " and did " + damage.ToString() + " damage.");
        //Apply damage:
        Health health = col.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        //Create particle for impact: 
        GameObject.Instantiate(impactParticle, col.contacts[0].point, Quaternion.LookRotation(col.contacts[0].normal), col.transform);
    }

}
