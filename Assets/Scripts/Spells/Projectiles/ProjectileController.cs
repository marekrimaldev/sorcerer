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
    [SerializeField] private float _homingOffset = 1f;
    [SerializeField] private float _maxOffset = .5f;
    [SerializeField] private float _maxAcc = .01f;

    private const float AccelerationMultiplier = 2;

    private Projectile _projectile;
    private Rigidbody _rb;
    private Transform _aim;
    private Transform _target;

    private Transform _targetOrientedAim;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = _applyGravity;

        _target = FindObjectOfType<TargetTransform>().transform;
        _targetOrientedAim = new GameObject("Target direction transform").transform;
    }

    private void OnDestroy()
    {
        if(_targetOrientedAim != null)
            Destroy(_targetOrientedAim.gameObject);
    }

    public void SendProjectile(float chargePercent, Projectile projectile, Transform aimTransform)
    {
        _aim = aimTransform;
        _projectile = projectile;

        float forceMultiplier = Mathf.Lerp(0.3f, 1, chargePercent);
        GetComponent<Rigidbody>()?.AddForce(transform.forward * projectile.Force * forceMultiplier);

        StartCoroutine(ControllProjectileCoroutine(forceMultiplier));
    }

    private IEnumerator ControllProjectileCoroutine(float forceMultiplier)
    {
        Vector3 lastPos = _aim.position;
        Vector3 homingAcc = Vector3.zero;
        Vector3 handAcc = Vector3.zero;
        Vector3 acceleration = Vector3.zero;

        Vector3 homingPos = _target.position + (_aim.position - _target.position).normalized * _homingOffset;

        _rb.transform.LookAt(_rb.transform.position + _rb.velocity);

        float playerTargetDistance = Vector3.Distance(_target.position, _aim.position);

        while (this)
        {
            yield return null;

            _targetOrientedAim.position = _aim.position;

            // Hand acceleration
            _targetOrientedAim.LookAt(_target);
            Vector3 offset = _targetOrientedAim.position - lastPos;
            offset = Vector3.ProjectOnPlane(offset, _targetOrientedAim.forward);
            handAcc = offset * AccelerationMultiplier * _projectile.Force * _handling;
            lastPos = _targetOrientedAim.position;

            // Homing with velocity
            Vector3 vecToPlayer = _aim.position - _rb.position;
            Vector3 vecToTarget = _target.position - _rb.position;
            Vector3 dirToTarget = vecToTarget.normalized;
            Vector3 velocity = _rb.velocity;
            float t = vecToPlayer.magnitude / playerTargetDistance;
            //velocity = Vector3.Lerp(velocity, dirToTarget * velocity.magnitude, t + _homingIntensity);

            Try to somehow bing handAcc to t

            if (t < 1)
            {
                velocity = Vector3.Lerp(velocity, dirToTarget * velocity.magnitude, t * _homingIntensity);
                _rb.velocity = velocity;
            }

            // Homing with acceleration
            //Vector3 vecToHand = _targetOrientedAim.position - _rb.position;
            //Vector3 vecToTarget = _target.position - _rb.position;
            //float ratio = vecToHand.magnitude / vecToTarget.magnitude;  // the further away the bigger
            //homingAcc = vecToTarget.normalized * _homingIntensity;
            //homingAcc = _rb.transform.InverseTransformVector(homingAcc);
            //Vector3.Scale(homingAcc, new Vector3(1/ratio, ratio, ratio));
            //homingAcc = _rb.transform.TransformVector(homingAcc);

            acceleration = homingAcc + handAcc;
            _rb.AddForce(acceleration, ForceMode.Acceleration);
        }
    }
}
