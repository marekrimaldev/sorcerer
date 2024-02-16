using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private InputActionProperty _triggerAction;
    [SerializeField] private InputActionProperty _gripAction;

    private void Update()
    {
        float triggerVal = _triggerAction.action.ReadValue<float>();
        float gripVal = _gripAction.action.ReadValue<float>();

        _handAnimator.SetFloat("Trigger", triggerVal);
        _handAnimator.SetFloat("Grip", gripVal);
    }
}
