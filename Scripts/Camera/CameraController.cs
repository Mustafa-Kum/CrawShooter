using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTransform;

    [SerializeField] private float turnSpeed = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = followTransform.position;
    }

    public void AddRotateInput(float value)
    {
        transform.Rotate(Vector3.up, value * Time.deltaTime * turnSpeed);
    }
}
