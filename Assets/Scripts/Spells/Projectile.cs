using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Projectile : HittableSpellCast
{
    [SerializeField] private float _force;
    [SerializeField][Range(0, 1)] private float _handling = 0.5f;      // 0 = we cant controll : 1 = full controll
    [SerializeField] private bool _applyGravity = true;
    [SerializeField] private VFXController _projectileVFXControllerPrefab;
    [SerializeField] private VFXController _impactVFXControllerPrefab;

    private const float AccelerationMultiplier = 2;

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

        StartCoroutine(ControllProjectileCoroutine(spell.AimIndicator.SpawnPoint));
    }

    private IEnumerator ControllProjectileCoroutine(Transform aimTransform)
    {
        Vector3 lastPos = aimTransform.position;
        Vector3 acceleration = Vector3.zero;

        _rb.transform.LookAt(_rb.transform.position + _rb.velocity);

        while (this)
        {
            yield return null;

            acceleration = (aimTransform.position - lastPos) * AccelerationMultiplier * _force * _handling;
            lastPos = aimTransform.position;

            _rb.AddForce(acceleration, ForceMode.Acceleration);
        }
    }
}
