using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class CameraArm : MonoBehaviour
{
    [SerializeField] private Transform child;
    
    [SerializeField] private float armLength;

    void Start()
    {
        
    }

    void Update()
    {
        child.position = transform.position - child.forward * armLength;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(child.position, transform.position);
    }
}
