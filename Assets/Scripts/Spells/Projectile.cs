using UnityEngine;

public class Projectile : HittableSpellCast
{
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _effectPrefab;

    private Rigidbody _rb;

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * _speed * Time.fixedDeltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        _onHitEffectManager.ApplyEffects(transform.position, other.transform);
        Destroy(gameObject);
    }

    public override void Init(Spell spell)
    {
        base.Init(spell);

        _rb = GetComponent<Rigidbody>();

        GameObject effect = Instantiate(_effectPrefab, transform.position, transform.rotation, transform);
    }
}
