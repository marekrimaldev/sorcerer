using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This class represents projectile in the terms of its visuals and attributes.
/// </summary>
public class Projectile : HittableSpell
{
    [SerializeField] private float _force;
    [SerializeField] private ProjectileController _controller;
    [SerializeField] private VFXController _projectileVFXControllerPrefab;
    [SerializeField] private VFXController _impactVFXControllerPrefab;

    public float Force => _force;

    private void OnCollisionEnter(Collision other)
    {
        VFXController vfx = Instantiate(_impactVFXControllerPrefab, transform.position, Quaternion.identity);
        vfx.DestroyVFXInSeconds(0.5f);

        _onHitEffectManager.ApplyEffects(other.GetContact(0).point, other.transform);
        Destroy(gameObject);
    }

    public override void Init(SpellCaster spell, float chargePercent = 1)
    {
        base.Init(spell);

        Instantiate(_projectileVFXControllerPrefab, transform.position, transform.rotation, transform);
        _controller.SendProjectile(chargePercent, this, spell.AimIndicator.SpawnPoint);
    }

    public override void Dispose()
    {
        _controller.StopAllCoroutines();

        base.Dispose();
    }
}
