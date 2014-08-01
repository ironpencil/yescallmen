using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour
{
    private Vector3 originPosition;
    public float shake_decay, shake_intensity;
    public float shake_delay = 0.001f;

    public float addShakeAmount = 0.0f;

    bool isShaking = false;

    // Use this for initialization
    void Start()
    {
        originPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Shake It Up")]
    public void StartShaking()
    {
        isShaking = true;
        originPosition = transform.position; //I think I used this in the title screen so have to leave it in

        StartCoroutine(DoShake());
    }

    public void ShakeWithoutUpdatePosition()
    {
        isShaking = true;

        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
        shake_intensity += addShakeAmount;

        while (shake_intensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            shake_intensity -= shake_decay;

            yield return new WaitForSeconds(shake_delay);

        }

        isShaking = false;
    }

    public void Shake(float intensity, float decay)
    {
        isShaking = true;
        shake_intensity = intensity;
        shake_decay = decay;

        StartCoroutine(DoShake());
    }
}
