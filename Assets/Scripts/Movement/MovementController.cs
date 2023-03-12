using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] protected MovementProvider _movementProvider;
    [SerializeField] private InputActionProperty _leftHandMoveAction;
    [SerializeField] private InputActionProperty _rightHandMoveAction;

    void Update()
    {
        Vector2 leftHandMove = _leftHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        Vector2 rightHandMove = _rightHandMoveAction.action?.ReadValue<Vector2>() ?? Vector2.zero;

        _movementProvider.LeftHandMove(leftHandMove);
        _movementProvider.RightHandMove(rightHandMove);
    }
}
