using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimatorContoller : MonoBehaviour
{
    [SerializeField] private InputActionProperty _triggerAction;
    [SerializeField] private InputActionProperty _gripAction;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float triggerVal = _triggerAction.action.ReadValue<float>();
        float gripVal = _gripAction.action.ReadValue<float>();

        _animator.SetFloat("Trigger", triggerVal);
        _animator.SetFloat("Grip", gripVal);
    }
}
