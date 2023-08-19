using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [SerializeField] private Transform shakeTransform;

    private Vector3 originalPosition;

    private Coroutine shakeCoroutine;
    
    [SerializeField] private float shakePower = 0.1f;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeRecoverySpeed = 10f;

    private bool isShaking;
 
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        ProcessShake();
    }

    public void StartShake()
    {
        if (shakeCoroutine == null)
        {
            isShaking = true;

            shakeCoroutine = StartCoroutine(ShakeStarted());
        }
    }

    IEnumerator ShakeStarted()
    {
        yield return new WaitForSeconds(shakeDuration);

        isShaking = false;

        shakeCoroutine = null;
    }

    private void ProcessShake()
    {
        if (isShaking)
        {
            Vector3 shakeValue = new Vector3(Random.value, Random.value, Random.value) * shakePower * (Random.value > 0.5 ? -1 : 1);

            shakeTransform.localPosition += shakeValue;
        }
        else
        {
            shakeTransform.localPosition = Vector3.Lerp(shakeTransform.localPosition, originalPosition, Time.deltaTime * shakeRecoverySpeed);
        }
    }
}
