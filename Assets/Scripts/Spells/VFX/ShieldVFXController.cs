using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldVFXController : VFXController
{
    [SerializeField] private MeshRenderer _shieldRenderer;

    public void SetShieldDirection(Vector3 dir)
    {
        _shieldRenderer.material.SetVector("_ShieldDirection", dir);
    }
}
