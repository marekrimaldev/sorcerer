using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMovementProvider : MovementProvider
{
    public override void LeftHandMove(Vector2 movement)
    {
        Vector3 direction = new Vector3(movement.x, 0, movement.y);
        direction = _headTransform.TransformDirection(direction);
        direction.y = 0;
        direction = direction.normalized;
        _rigTransform.transform.position += direction * _speed * Time.deltaTime;
    }

    public override void RightHandMove(Vector2 movement)
    {
        Vector3 direction = new Vector3(movement.x, 0, movement.y);
        direction = _headTransform.TransformDirection(direction);
        direction.y = 0;
        direction = direction.normalized;
        _rigTransform.transform.position += direction * _speed * Time.deltaTime;
    }
}
