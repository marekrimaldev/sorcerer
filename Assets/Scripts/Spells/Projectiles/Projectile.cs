using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// This class represents projectile in the terms of its visuals and attributes.
/// </summary>
[RequireComponent(typeof(ProjectileController))]
public class Projectile : HittableSpell
{
    [SerializeField] private VFXController _projectileVFXControllerPrefab;
    [SerializeField] private VFXController _impactVFXControllerPrefab;
    [SerializeField] protected ProjectileData _data;
    public ProjectileData Data => _data;

    private ProjectileController _controller;
    private int _bouncesLeft;

    private void OnCollisionEnter(Collision other)
    {
        if (_bouncesLeft-- > 0)
        {
            _controller.DoBounce(other);
            return;
        }

        VFXController vfx = Instantiate(_impactVFXControllerPrefab, transform.position, Quaternion.identity);
        vfx.DestroyVFXInSeconds(0.5f);

        _onHitEffectManager.ApplyEffects(other.GetContact(0).point, other.transform);
        Dispose();
    }

    public override void Init(SpellCaster spellCaster)
    {
        base.Init(spellCaster);

        Instantiate(_projectileVFXControllerPrefab, transform.position, transform.rotation, transform);

        _bouncesLeft = _data.BounceCount;

        _controller = GetComponent<ProjectileController>();
        _controller.SendProjectile(this, spellCaster.AimIndicator.SpawnPoint);
    }

    public override void Dispose()
    {
        StopAllCoroutines();
        _controller.Dispose();

        base.Dispose();
    }
}
