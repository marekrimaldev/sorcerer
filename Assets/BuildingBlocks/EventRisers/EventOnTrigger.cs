using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    private enum TriggerState { Enter, Stay, Exit}
    [SerializeField] private TriggerState _triggerState;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private VoidGameEvent OnTrigger;
    [SerializeField] private UnityEvent OnTriggerUE;

    [Tooltip("Disables this trigger after triggering for disableTime seconds")]
    [SerializeField] private float _disableTime = 0.05f;
    private float _nextTriggerTime = 0;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanTrigger(collision.gameObject) && _triggerState == TriggerState.Enter)
            RaiseEvent();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CanTrigger(collision.gameObject) && _triggerState == TriggerState.Stay)
            RaiseEvent();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CanTrigger(collision.gameObject) && _triggerState == TriggerState.Exit)
            RaiseEvent();
    }

    private bool CanTrigger(GameObject go)
    {
        if ((_layerMask.value & (1 << go.layer)) < 1)
            return false;

        if (Time.time < _nextTriggerTime)
            return false;

        return true;
    }

    private void RaiseEvent()
    {
        Void v;
        OnTrigger?.Raise(v);
        OnTriggerUE?.Invoke();

        _nextTriggerTime = Time.time + _disableTime;
    }
}
