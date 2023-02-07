using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _effectPrefab;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameObject effect = Instantiate(_effectPrefab, transform.position, transform.rotation);
        effect.transform.SetParent(transform);
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * _speed * Time.fixedDeltaTime;
    }
}
