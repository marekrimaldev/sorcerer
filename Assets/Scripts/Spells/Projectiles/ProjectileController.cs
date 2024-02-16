using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls projectile trajectory.
/// </summary>
public class ProjectileController : MonoBehaviour
{
    protected Transform _hommingAimIndicator;
    protected Projectile _projectile;
    protected Rigidbody _rb;
    protected Transform _aim;
    protected HommingTarget _hommingTarget;
    protected Transform _targetOrientedAim;

    public enum ControlType { HandOnly, HomingAim }

    public void Dispose()
    {
        if(_hommingAimIndicator != null)
            Destroy(_hommingAimIndicator.gameObject);
        if (_targetOrientedAim != null)
            Destroy(_targetOrientedAim.gameObject);
    }

    public void DoBounce(Collision other)
    {
        _rb.velocity = new Vector3(_rb.velocity.x, -_projectile.Data.Force / 500, _rb.velocity.z);
        Debug.Log("_rb.velocity: " + _rb.velocity);
        _rb.velocity = Vector3.Reflect(_rb.velocity, other.contacts[0].normal);
        Debug.Log("_rb.velocity 2: " + _rb.velocity);
    }

    public void SendProjectile(Projectile projectile, Transform aimTransform)
    {
        _aim = aimTransform;
        _projectile = projectile;
        _rb = _projectile.GetComponent<Rigidbody>();
        _rb.useGravity = projectile.Data.ApplyGravity;

        float forceMultiplier = Mathf.Lerp(0.3f, 1, projectile.Gauntlet.ChargeInfo.percent);
        _rb.AddForce(_projectile.transform.forward * projectile.Data.Force * forceMultiplier);

        _targetOrientedAim = new GameObject("Target direction transform").transform;
        _hommingTarget = FindObjectOfType<HommingTarget>();

        if (projectile.Data.UseHomming)
        {
            _hommingAimIndicator = Instantiate(projectile.Data.HommingAimIndicatorPrefab).transform;
            _projectile.StartCoroutine(ControllProjectileCoroutineHomming(forceMultiplier));
        }
        else
        {
            _projectile.StartCoroutine(ControllProjectileCoroutineHandControll(forceMultiplier));
        }
    }

    // TODO: Somehow unify those two coroutines so it can be combined
    private IEnumerator ControllProjectileCoroutineHandControll(float forceMultiplier)
    {
        yield return new WaitForSeconds(0.05f);

        Vector3 lastPos = _aim.position;
        Vector3 handAcc = Vector3.zero;
        _rb.transform.LookAt(_rb.transform.position + _rb.velocity);

        Vector3 chargeAcc = _projectile.Gauntlet.ChargeInfo.GetAcceleration() * _projectile.Data.AccelerationSpeed;

        while (_projectile)
        {
            yield return null;

            _targetOrientedAim.position = _aim.position;

            // Hand acceleration
            _targetOrientedAim.LookAt(_hommingTarget.transform.position);
            Vector3 offset = _targetOrientedAim.position - lastPos;
            offset = Vector3.ProjectOnPlane(offset, _targetOrientedAim.forward);
            offset.y *= .25f;
            handAcc = offset * _projectile.Data.Force * _projectile.Data.AccelerationSpeed;
            float accMagnitude = Mathf.Min(handAcc.magnitude, _projectile.Data.MaxAcceleration);
            handAcc = handAcc.normalized * accMagnitude;
            lastPos = _targetOrientedAim.position;

            // Charge acc
            _rb.AddForce(chargeAcc);
        }
    }

    private IEnumerator ControllProjectileCoroutineHomming(float forceMultiplier)
    {
        yield return new WaitForSeconds(0.05f);

        Vector3 lastPos = _aim.position;
        Vector3 handAcc = Vector3.zero;
        _rb.transform.LookAt(_rb.transform.position + _rb.velocity);

        float playerTargetDistance = Vector3.Distance(_hommingTarget.HommingPoint, _aim.position);
        Vector3 startVelocity = _rb.velocity;
        Vector3 startDir = _rb.velocity.normalized;
        _hommingAimIndicator.position = _hommingTarget.HommingPoint;

        while (_projectile)
        {
            yield return null;

            _targetOrientedAim.position = _aim.position;

            float distanceToTarget = (_rb.position - _hommingTarget.HommingPoint).magnitude;
            if (distanceToTarget < _hommingTarget.HommingRadius)
            {
                Dispose();
                yield break;
            }

            // Hand acceleration
            _targetOrientedAim.LookAt(_hommingAimIndicator);
            Vector3 offset = _targetOrientedAim.position - lastPos;
            lastPos = _targetOrientedAim.position;
            _hommingAimIndicator.position += offset * _projectile.Data.HomingAccuracy;

            // Bound aim indicator
            Vector3 vecToHommingPoint = (_hommingAimIndicator.position - _hommingTarget.HommingPoint);
            float distToHommingPoint = vecToHommingPoint.magnitude;
            distToHommingPoint = Mathf.Min(distToHommingPoint, _hommingTarget.HommingRadius);
            _hommingAimIndicator.position = _hommingTarget.HommingPoint + vecToHommingPoint.normalized * distToHommingPoint;

            // Homing with velocity
            Vector3 vecToPlayer = _aim.position - _rb.position;
            Vector3 vecToTarget = _hommingAimIndicator.position - _rb.position;
            Vector3 dirToTarget = vecToTarget.normalized;
            float t = vecToPlayer.magnitude / playerTargetDistance;

            _rb.velocity = Vector3.Lerp(startDir, dirToTarget, t * _projectile.Data.HomingIntensity) * startVelocity.magnitude;
        }
    }
}
