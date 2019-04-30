using UnityEngine;
using System.Collections;

using Helper;

public class CameraShake : MonoBehaviour
{

    [SerializeField]
    private float shakeTime = 0.0f;
    [SerializeField]
    private float shakeAmount = 0.0f;
    private GameObject player;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag(GameTag.Player);
        if (player != null)
        {
            PlayerAttack pa = player.GetComponent<PlayerAttack>();
            if (pa != null)
            {
                pa.OnShakeCamera += Shake;
            }
        }
	}

    void OnDestroy()
    {
        //Remove all event subscriptions
        if (player != null)
        {
            PlayerAttack pa = player.GetComponent<PlayerAttack>();
            pa.OnShakeCamera -= Shake;
        }
    }

    public void Shake()
    {

        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        float currentTime = 0f;
        while (currentTime <= shakeTime)
        {
            currentTime += Time.deltaTime;
            transform.localPosition = Random.insideUnitSphere * shakeAmount;
            //shakeTime -= Time.deltaTime * shakeDecreaseFactor;
            yield return 0;
        }
        transform.localPosition = Vector3.zero;
    }
}
