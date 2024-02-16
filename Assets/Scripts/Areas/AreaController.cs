using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    [SerializeField] private GameObject _areaPrefab;
    [SerializeField] private Transform _areaPosition;
    private GameObject _areaInstance;

    private void Start()
    {
        RestartArea();
    }

    private void RestartArea()
    {
        if(_areaInstance != null)
            Destroy(_areaInstance);

        _areaInstance = Instantiate(_areaPrefab, _areaPosition.position, _areaPosition.rotation, _areaPosition);
        _areaInstance.transform.localPosition = Vector3.zero;
        _areaInstance.transform.localRotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        RestartArea();
    }

    private void OnCollisionEnter(Collision collision)
    {
        RestartArea();
    }
}
