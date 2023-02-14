using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnHitExplosion", menuName = "On Hit Effects/Explosion", order = 1)]
public class OnHitExplosion : OnHitEffect
{
    [SerializeField] private float _explosionForce = 500;
    [SerializeField] private float _explosionRadius = 3;

    public override void ApplyEffect(Vector3 hitPoint, Transform hitTransform)
    {
        Collider[] cols = Physics.OverlapSphere(hitPoint, _explosionRadius);
        for (int i = 0; i < cols.Length; i++)
        {
            Rigidbody rb = cols[i].attachedRigidbody;
            if(rb != null)
            {
                rb.AddExplosionForce(_explosionForce, hitPoint, _explosionRadius);
            }
        }
    }
}
