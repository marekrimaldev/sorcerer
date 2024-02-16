using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDisplay : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _activationAngle = 45;
    [SerializeField] private GameObject _ui;

    private void Update()
    {
        _ui.SetActive(IsDisplayFacingCamera());
    }

    private bool IsDisplayFacingCamera()
    {
        Vector3 vecToCamera = _camera.position - transform.position;
        float angle = Vector3.Angle(transform.forward, vecToCamera);
        return Mathf.Abs(angle) < _activationAngle;
    }

}
