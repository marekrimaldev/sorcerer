using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Toggler : MonoBehaviour
{
    [SerializeField] private bool _value = false;
    [SerializeField] private bool _raiseEventOnStart = false;

    [SerializeField] private UnityEvent OnToggleOn;
    [SerializeField] private UnityEvent OnToggleOff;

    private void Start()
    {
        if(_raiseEventOnStart)
            RaiseEvent();
    }

    public void Toggle()
    {
        _value = !_value;
        RaiseEvent();
    }

    private void RaiseEvent()
    {
        if (_value)
            OnToggleOn?.Invoke();
        else
            OnToggleOff?.Invoke();
    }
}
