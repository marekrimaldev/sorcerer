using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnHitEffect
{
    void ApplyEffect(Vector3 hitPoint, Transform hitTransform);
}
