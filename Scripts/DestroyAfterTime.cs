using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{

    [SerializeField]
    private float delay;

	// Use this for initialization
	void Start ()
    {
        Destroy(this.gameObject, delay);
	}
	
}
