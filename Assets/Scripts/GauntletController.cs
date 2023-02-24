using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GauntletController : MonoBehaviour
{
    [SerializeField] private Gauntlet _gauntletPrefab;

    [SerializeField] private InputActionProperty _gripCast;
    [SerializeField] private InputActionProperty _triggerCast;

    private Gauntlet _gauntlet;

    bool _gripPressed = false;
    bool _triggerPressed = false;

    const float Threshold = 0.1f;

    private void Awake()
    {
        _gauntlet = Instantiate(_gauntletPrefab, transform) as Gauntlet;
        _gauntlet.transform.localPosition = Vector3.zero;
        _gauntlet.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        float triggerCastVal = _triggerCast.action.ReadValue<float>();
        float gripCastVal = _gripCast.action.ReadValue<float>();

        if (!_gripPressed && gripCastVal > Threshold)
        {
            _gauntlet.PrimaryPress();
            _gripPressed = true;
        }
        else if (_gripPressed && gripCastVal < Threshold)
        {
            _gauntlet.PrimaryRelease();
            _gripPressed = false;
        }

        if (!_triggerPressed && triggerCastVal > Threshold)
        {
            _gauntlet.SecondaryPress();
            _triggerPressed = true;
        }
        else if (_triggerPressed && triggerCastVal < Threshold)
        {
            _gauntlet.SecondaryRelease();
            _triggerPressed = false;
        }
    }
}
