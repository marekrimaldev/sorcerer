using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresencePhysics : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = (_target.position - transform.position) / Time.fixedDeltaTime;
        
        Quaternion rotationDiff = _target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDiff.ToAngleAxis(out float angleInDegrees, out Vector3 rotations);
        Vector3 rotationsInDegrees = angleInDegrees * rotations;
        _rb.angularVelocity = rotationsInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime;

    }
}
