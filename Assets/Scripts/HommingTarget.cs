using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HommingTarget : MonoBehaviour
{
    [SerializeField] private Transform _hommingSphere;
    public Vector3 HommingPoint => _hommingSphere.position;
    public float HommingRadius => _hommingSphere.lossyScale.x / 2f;
}
