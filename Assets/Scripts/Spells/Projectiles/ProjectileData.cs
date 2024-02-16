using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Projectiles/Projectile Data", order = 1)]
public class ProjectileData : ScriptableObject
{
    [SerializeField] private float _force = 3000;
    [SerializeField] private bool _applyGravity = true;
    [SerializeField] private int _bounceCount = 1;
    [SerializeField] [Range(0, 5)] private float _accelerationSpeed = 0.5f;
    [SerializeField] [Range(0, 100)] private float _maxAcceleration = 50f;
    [SerializeField] private bool _useHomming = false;
    [SerializeField] protected float _homingIntensity = 1f;
    [SerializeField] private float _homingAccuracy = 1f;
    [SerializeField] private float _hommingRadius = 1f;
    [SerializeField] private GameObject _hommingAimIndicatorPrefab;

    public float Force => _force;
    public bool ApplyGravity => _applyGravity;
    public int BounceCount => _bounceCount;
    public bool UseHomming => _useHomming;
    public float AccelerationSpeed => _accelerationSpeed;
    public float MaxAcceleration => _maxAcceleration;
    public float HomingIntensity => _homingIntensity;
    public float HomingAccuracy => _homingAccuracy;
    public float HommingRadius => _hommingRadius;
    public GameObject HommingAimIndicatorPrefab => _hommingAimIndicatorPrefab;

}
