using UnityEngine;
using System.Collections;

public class WeaponDamageNonPhysics_Placeholder : MonoBehaviour
{
    public int damage = 5;
    public float forceAmount = 10f;
    public GameObject impactParticle;

    private Vector3 previousPosition;
    private Vector3 directionOfMovement;


    private void LateUpdate()
    {
        directionOfMovement = Vector3.Normalize(previousPosition - transform.position);
        previousPosition = transform.position;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.CompareTag("Player"))
        {
            //Hit something!
            Debug.Log(gameObject.name + " hit " + col.gameObject.name + " and did " + damage.ToString() + " damage.");
            //Get position of hit: 
            bool foundCollisionPoint = false;
            //Vector3 direction = Vector3.Normalize(col.transform.position - transform.position);
            Vector3 hitPos = Vector3.zero, normal = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionOfMovement, out hit))
            {
                //if (hit.collider == col)
                //{
                    foundCollisionPoint = true;
                    hitPos = hit.point;
                    normal = hit.normal;
                    Debug.Log("Point of contact: " + hit.point);
                //}
            }
            //Apply force to the other object:
            if (col.attachedRigidbody != null)
            {
                if (foundCollisionPoint)
                    col.attachedRigidbody.AddForceAtPosition(directionOfMovement * forceAmount, hitPos);
                else
                    col.attachedRigidbody.AddForce(directionOfMovement * forceAmount);
            }
            //Create particle for impact: 
            if (foundCollisionPoint)
                GameObject.Instantiate(impactParticle, hitPos, Quaternion.LookRotation(hit.normal), col.transform);
        }
    }

}
