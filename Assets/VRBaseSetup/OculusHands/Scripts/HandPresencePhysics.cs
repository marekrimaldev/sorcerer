using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresencePhysics : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _realHand;
    [SerializeField] private float _showRealHandDistance = .1f;
    
    private Rigidbody _rb;
    private Collider[] _colliders;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }
    public void EnableCollidersDelayed(float delay)
    {
        StartCoroutine(EnableCollidersInSeconds(delay));
    }

    private IEnumerator EnableCollidersInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        EnableColliders(true);
    }

    public void EnableColliders(bool value)
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = value;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = (_target.position - transform.position) / Time.fixedDeltaTime;
        
        Quaternion rotationDiff = _target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDiff.ToAngleAxis(out float angleInDegrees, out Vector3 rotations);
        Vector3 rotationsInDegrees = angleInDegrees * rotations;
        _rb.angularVelocity = rotationsInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime;

        _realHand.SetActive(Vector3.Distance(_target.position, transform.position) > _showRealHandDistance);
    }
}
