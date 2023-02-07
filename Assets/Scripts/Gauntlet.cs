using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _palmSpawnPosition;
    [SerializeField] private Transform _fingerSpawnPosition;

    public void InstantCast()
    {
        Debug.Log("Instant cast");
        Instantiate(_projectilePrefab, _fingerSpawnPosition.position, _fingerSpawnPosition.rotation);
    }

    public void ChargingBegin()
    {
        Debug.Log("Charging begin");
    }

    public void ChargingEnd()
    {
        Debug.Log("Charging end");
        Instantiate(_projectilePrefab, _palmSpawnPosition.position, _palmSpawnPosition.rotation);
    }
}
