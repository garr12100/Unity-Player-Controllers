using UnityEngine;
using System.Collections;

using Helper;


/// <summary>
/// Attack box that is spawned on attack, damages all enemies in range. 
/// </summary>
public class AttackBox : MonoBehaviour {

    private int damage;

	// Use this for initialization
	void Start ()
    {
        Destroy(this.gameObject, .1f);
	}

    public void SetDamage(int dam)
    {
        damage = dam;
    }

    void OnTriggerEnter(Collider otherCol)
    {
        if (otherCol.CompareTag(GameTag.Enemy))
        {
            AnimHealth hp = otherCol.GetComponent<AnimHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }
    }
}
