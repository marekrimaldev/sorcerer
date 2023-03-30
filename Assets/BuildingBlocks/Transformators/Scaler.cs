using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float _scaleSpeed = 1;
    [SerializeField] private bool _startAutomaticaly = false;
    private bool _isScaling = false;

    private Coroutine _scaleCoroutine;

    private void Start()
    {
        if (_startAutomaticaly)
            StartScale();
    }

    public void StartScale()
    {
        if (_isScaling)
            return;

        _scaleCoroutine = StartCoroutine(Scale());
        _isScaling = true;
    }

    public void StopRotation()
    {
        if (_scaleCoroutine != null)
            StopCoroutine(_scaleCoroutine);

        _isScaling = false;
    }

    private IEnumerator Scale()
    {
        while (gameObject.activeSelf)
        {
            transform.localScale += new Vector3(1, 1, 1) * _scaleSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
