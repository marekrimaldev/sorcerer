using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnHitEffect : ScriptableObject, IOnHitEffect
{
    public abstract void ApplyEffect(Vector3 hitPoint, Transform hitTransform);
}
