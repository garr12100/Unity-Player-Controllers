using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTransform : MonoBehaviour
{
    public Transform toMatch;
    public bool matchPosition = true;
    public bool matchRotation = true;
    public bool matchScale = true;
	
	// Update is called once per frame
	void Update ()
    {
        if (matchPosition)
            transform.position = toMatch.position;
        if (matchRotation)
            transform.rotation = toMatch.rotation;
        if (matchScale)
            transform.localScale = toMatch.localScale;
    }
}
