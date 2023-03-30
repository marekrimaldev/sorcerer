using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnKeyPress : EventOnKeyPressBase
{
    [SerializeField] private VoidGameEvent OnKeyPress;
    [SerializeField] private UnityEvent OnKeyPressUE;

    protected override void InvokeEvent()
    {
        Void v;
        OnKeyPress?.Raise(v);
        OnKeyPressUE?.Invoke();
    }
}
