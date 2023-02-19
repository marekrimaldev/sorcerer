using UnityEngine;

public class Projectile : HittableSpellCast
{
    [SerializeField] private float _force;
    [SerializeField] private bool _applyGravity = true;
    [SerializeField] private GameObject _effectPrefab;
    [SerializeField] private GameObject _impactEffectPrefab;

    private Rigidbody _rb;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_impactEffectPrefab, transform.position, transform.rotation);

        _onHitEffectManager.ApplyEffects(transform.position, other.transform);
        Destroy(gameObject);
    }

    public override void Init(Spell spell, float chargePercent = 1)
    {
        base.Init(spell);

        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = _applyGravity;

        GameObject effect = Instantiate(_effectPrefab, transform.position, transform.rotation, transform);

        float forceMultiplier = Mathf.Lerp(0.3f, 1, chargePercent);
        _rb.AddForce(transform.forward * _force * forceMultiplier);
    }
}
