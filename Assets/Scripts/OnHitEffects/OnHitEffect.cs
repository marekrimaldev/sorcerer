using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitEffect : ScriptableObject, IOnHitEffect
{
    [SerializeField] private LayerMask _layerMask;

    protected bool ShouldApplyEffect(Transform hitTransform)
    {
        return ((_layerMask.value & (1 << hitTransform.gameObject.layer)) > 0);
    }

    public abstract void ApplyEffect(Vector3 hitPoint, Transform hitTransform);
}
