using UnityEngine;
using UnityEngine.VFX;

public class Projectile : HittableSpellCast
{
    [SerializeField] private float _force;
    [SerializeField] private bool _applyGravity = true;
    [SerializeField] private VFXController _projectileVFXControllerPrefab;
    [SerializeField] private VFXController _impactVFXControllerPrefab;

    private Rigidbody _rb;

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

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = _applyGravity;

        Instantiate(_projectileVFXControllerPrefab, transform.position, transform.rotation, transform);

        float forceMultiplier = Mathf.Lerp(0.3f, 1, chargePercent);
        _rb.AddForce(transform.forward * _force * forceMultiplier);
    }
}
