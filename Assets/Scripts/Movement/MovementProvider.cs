using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementProvider : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] protected Transform _rigTransform;
    [SerializeField] protected Transform _headTransform;

    public abstract void LeftHandMove(Vector2 movement);
    public abstract void RightHandMove(Vector2 movement);
}
