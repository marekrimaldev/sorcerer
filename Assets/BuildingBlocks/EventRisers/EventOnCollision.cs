using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnCollision : MonoBehaviour
{
    private enum CollisionState { Enter, Stay, Exit}
    [SerializeField] private CollisionState _collisionState;

    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private VoidGameEvent OnCollision;
    [SerializeField] private UnityEvent OnCollisionUE;

    [Tooltip("Disables collisions after colliding for disableTime seconds")]
    [SerializeField] private float _disableTime = 0.05f;
    private float _nextCollisionTime = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision enter");

        if (CanCollide(collision.gameObject) && _collisionState == CollisionState.Enter)
            RaiseEvent();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (CanCollide(collision.gameObject) && _collisionState == CollisionState.Stay)
            RaiseEvent();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (CanCollide(collision.gameObject) && _collisionState == CollisionState.Exit)
            RaiseEvent();
    }

    private bool CanCollide(GameObject go)
    {
        if ((_layerMask.value & (1 << go.layer)) < 1)
            return false;

        if (Time.time < _nextCollisionTime)
            return false;

        return true;
    }

    private void RaiseEvent()
    {
        Void v;
        OnCollision?.Raise(v);
        OnCollisionUE?.Invoke();

        _nextCollisionTime = Time.time + _disableTime;
    }
}
