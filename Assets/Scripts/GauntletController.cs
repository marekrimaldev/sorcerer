using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GauntletController : MonoBehaviour
{
    [SerializeField] private Gauntlet _gauntletPrefab;

    [SerializeField] private InputActionProperty _gripCast;
    [SerializeField] private InputActionProperty _triggerCast;
    [SerializeField] private InputActionProperty _primaryButtonCast;
    [SerializeField] private InputActionProperty _secondaryButtonCast;

    private Gauntlet _gauntlet;

    bool _gripPressed = false;
    bool _triggerPressed = false;
    bool _primaryButtonPressed = false;
    bool _secondaryButtonPressed = false;

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
        //float primaryButtonCast = _primaryButtonCast.action.ReadValue<float>();
        //bool primaryButtonCastBool = _primaryButtonCast.action.ReadValue<bool>();
        //bool secondaryButtonCast = _secondaryButtonCast.action.ReadValue<bool>();

        //Debug.Log(primaryButtonCast);
        //Debug.Log(primaryButtonCastBool);

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

        //if (!_primaryButtonPressed && primaryButtonCast > 0.1f)
        //{
        //    _gauntlet.ShieldPress();
        //    _primaryButtonPressed = true;
        //}
        //else if (_primaryButtonPressed && primaryButtonCast < 0.1f)
        //{
        //    _gauntlet.ShieldRelease();
        //    _primaryButtonPressed = false;
        //}
    }
}
