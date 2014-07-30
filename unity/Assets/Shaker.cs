using UnityEngine;
using System.Collections;

public class Shaker : MonoBehaviour
{
    private Vector3 originPosition;
    public float shake_decay, shake_intensity;
    public float shake_delay = 0.001f;   

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

    public void StartShaking()
    {
        isShaking = true;
        originPosition = transform.position;

        StartCoroutine(DoShake());
    }

    private IEnumerator DoShake()
    {
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
        shake_intensity = intensity;
        shake_decay = decay;
    }
}
