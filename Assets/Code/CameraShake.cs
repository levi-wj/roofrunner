using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Transform player = null;
    [SerializeField] float offset = 1f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float threshold = 1f;

    private float shakeLength = 0f;
    private float shakeIntensity = 0.7f;

    // Update is called once per frame
    void Update()
    {
        if (shakeLength > 0f) {
            transform.localPosition = new Vector3(
                Random.insideUnitCircle.x * shakeIntensity,
                Random.insideUnitCircle.y * shakeIntensity,
                transform.position.z);

            shakeLength -= Time.deltaTime;
        } else {
            shakeLength = 0f;
            transform.localPosition = new Vector3(0, 0 , transform.position.z);
        }
    }

    public void SetCameraShake(float length, float intensity)
    {
        shakeLength = length;
        shakeIntensity = intensity;
    }
}
