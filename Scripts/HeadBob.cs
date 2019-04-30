using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour {

    public float headbobSpeed = 1f;
    public float headbobAmountX = 1f;
    public float headbobAmountY = 1f;
    public float eyeHeightRatio = 0.9f;

    private float headbobStepCounter;
    private Vector3 parentLastPos;
    private Grounder grounder;
    // Use this for initialization
    void Awake ()
    {
        if(grounder == null)
            grounder = transform.parent.GetComponent<Grounder>();
        parentLastPos = transform.parent.transform.position;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (grounder.Grounded)
            headbobStepCounter += Vector3.Distance(parentLastPos, transform.parent.transform.position) * headbobSpeed;
        float x = Mathf.Sin(headbobStepCounter) * headbobAmountX;
        float y = (Mathf.Cos(headbobStepCounter * 2) * -headbobAmountY) + (transform.parent.localScale.y * eyeHeightRatio) - (transform.localScale.y / 2f);
        transform.localPosition = new Vector3(x, y, transform.localPosition.z);
        parentLastPos = transform.parent.transform.position;

    }
}
