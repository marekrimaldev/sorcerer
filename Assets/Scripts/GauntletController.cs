using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GauntletController : MonoBehaviour
{
    [SerializeField] private Gauntlet _gauntlet;

    [SerializeField] private InputActionProperty _triggerCast;
    [SerializeField] private InputActionProperty _gripCast;

    bool _triggerPressed = false;
    bool _gripPressed = false;

    const float Threshold = 0.01f;

    private void Update()
    {
        float triggerCastVal = _triggerCast.action.ReadValue<float>();
        float gripCastVal = _gripCast.action.ReadValue<float>();

        if (!_triggerPressed && triggerCastVal > Threshold)
        {
            _gauntlet.PrimaryPress();
            _triggerPressed = true;
        }
        else if (_triggerPressed && triggerCastVal < Threshold)
        {
            _gauntlet.PrimaryRelease();
            _triggerPressed = false;
        }

        if (!_gripPressed && gripCastVal > Threshold)
        {
            _gauntlet.SecondaryPress();
            _gripPressed = true;
        }
        else if (_gripPressed && gripCastVal < Threshold)
        {
            _gauntlet.SecondaryRelease();
            _gripPressed = false;
        }
    }
}
