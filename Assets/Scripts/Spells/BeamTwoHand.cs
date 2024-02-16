using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamTwoHand : Beam
{
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;

    protected override Vector3 GetBeamEnd()
    {
        Vector3 right = GetRightVector();
        Vector3 up = Vector3.up;
        Vector3 forward = Vector3.Cross(up, right).normalized;

        return forward * _beamLength;
    }

    private Vector3 GetRightVector()
    {
        Vector3 right = _rightHand.position - _leftHand.position;
        right.y = 0;
        right = right.normalized;

        return right;
    }
}
