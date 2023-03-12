using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovementProvider : MovementProvider
{
    [SerializeField] protected Transform _centerTransform;

    public override void LeftHandMove(Vector2 movement)
    {
        _rigTransform.LookAt(_centerTransform);

        Vector3 direction = new Vector3(movement.x, 0, 0);
        direction = _rigTransform.TransformDirection(direction);
        direction.y = 0;
        direction = direction.normalized;
        _rigTransform.transform.position += direction * _speed * Time.deltaTime;
    }

    public override void RightHandMove(Vector2 movement)
    {
        _rigTransform.LookAt(_centerTransform);

        Vector3 direction = new Vector3(movement.x, 0, 0);
        direction = _rigTransform.TransformDirection(direction);
        direction.y = 0;
        direction = direction.normalized;
        _rigTransform.transform.position += direction * _speed * Time.deltaTime;
    }
}
