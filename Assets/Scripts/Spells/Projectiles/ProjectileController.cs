using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls projectile trajectory.
/// </summary>
public class ProjectileController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float _handling = 0.5f;      // 0 = we cant controll : 1 = full controll
    [SerializeField] private bool _applyGravity = true;
    [SerializeField] private float _homingIntensity = 1f;
    [SerializeField] private float _homingAccuracy = 1f;
    [SerializeField] private float _hommingRadius = 1f;

    [SerializeField] private GameObject _hommingTargetPrefab;
    private Transform _hommingTarget;

    private const float AccelerationMultiplier = 2;

    private Projectile _projectile;
    private Rigidbody _rb;
    private Transform _aim;
    private Transform _target;

    private Transform _targetOrientedAim;

    private void Awake()
    {
        _target = FindObjectOfType<TargetTransform>().transform;
        _targetOrientedAim = new GameObject("Target direction transform").transform;

        _hommingTarget = Instantiate(_hommingTargetPrefab).transform;
    }

    private void OnDestroy()
    {
        if(_targetOrientedAim != null)
            Destroy(_targetOrientedAim.gameObject);

        Destroy(_hommingTarget.gameObject);
    }

    public void SendProjectile(float chargePercent, Projectile projectile, Transform aimTransform)
    {
        _aim = aimTransform;
        _projectile = projectile;
        _rb = _projectile.GetComponent<Rigidbody>();
        _rb.useGravity = _applyGravity;

        float forceMultiplier = Mathf.Lerp(0.3f, 1, chargePercent);
        GetComponent<Rigidbody>()?.AddForce(transform.forward * projectile.Force * forceMultiplier);

        StartCoroutine(ControllProjectileCoroutine(forceMultiplier));
    }

    private IEnumerator ControllProjectileCoroutine(float forceMultiplier)
    {
        yield return new WaitForSeconds(0.05f);

        Vector3 lastPos = _aim.position;
        Vector3 handAcc = Vector3.zero;
        _rb.transform.LookAt(_rb.transform.position + _rb.velocity);

        float playerTargetDistance = Vector3.Distance(_target.position, _aim.position);
        Vector3 startVelocity = _rb.velocity;
        Vector3 startDir = _rb.velocity.normalized;
        _hommingTarget.position = _target.position;

        while (this)
        {
            yield return null;

            _targetOrientedAim.position = _aim.position;

            float distanceToTarget = (_rb.position - _target.position).magnitude;
            Vector3 hommingPosition = distanceToTarget < _hommingRadius ? _target.position : _hommingTarget.position;

            // Hand acceleration
            _targetOrientedAim.LookAt(_hommingTarget);
            Vector3 offset = _targetOrientedAim.position - lastPos;
            offset = Vector3.ProjectOnPlane(offset, _targetOrientedAim.forward);
            handAcc = offset * AccelerationMultiplier * _projectile.Force * _handling;
            lastPos = _targetOrientedAim.position;
            _hommingTarget.position += offset * _homingAccuracy;

            // Homing with velocity
            Vector3 vecToPlayer = _aim.position - _rb.position;
            Vector3 vecToTarget = hommingPosition - _rb.position;
            Vector3 dirToTarget = vecToTarget.normalized;
            float t = vecToPlayer.magnitude / playerTargetDistance;

            Vector3 velocity = handAcc;
            velocity += Vector3.Lerp(startDir, dirToTarget, t * _homingIntensity) * startVelocity.magnitude;
            _rb.velocity = velocity;
        }
    }
}
