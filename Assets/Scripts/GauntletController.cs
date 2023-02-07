using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GauntletController : MonoBehaviour
{
    [SerializeField] private Gauntlet _gauntlet;

    [SerializeField] private InputActionProperty _instantCast;
    [SerializeField] private InputActionProperty _chargeCast;

    bool _isCharging = false;

    const float Threshold = 0.01f;

    private void Update()
    {
        float instantCastVal = _instantCast.action.ReadValue<float>();
        float chargeCastVal = _chargeCast.action.ReadValue<float>();

        if (instantCastVal > Threshold)
        {
            _gauntlet.InstantCast();
        }

        if (!_isCharging && chargeCastVal > Threshold)
        {
            _gauntlet.ChargingBegin();
            _isCharging = true;
        }
        else if (_isCharging && chargeCastVal < Threshold)
        {
            _gauntlet.ChargingEnd();
            _isCharging = false;
        }
    }
}
