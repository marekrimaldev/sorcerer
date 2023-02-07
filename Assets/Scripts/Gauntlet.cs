using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private GameObject _chargeUpPrefab;
    [SerializeField] private Transform _palmSpawnPosition;
    [SerializeField] private Transform _fingerSpawnPosition;

    private GameObject _chargeUp;

    public void InstantCast()
    {
        Debug.Log("Instant cast");
        Instantiate(_projectilePrefab, _fingerSpawnPosition.position, _fingerSpawnPosition.rotation);
    }

    public void ChargingBegin()
    {
        Debug.Log("Charging begin");
        _chargeUp = Instantiate(_chargeUpPrefab, _palmSpawnPosition.position, _palmSpawnPosition.rotation);
        _chargeUp.transform.SetParent(_palmSpawnPosition);
    }

    public void ChargingEnd()
    {
        Debug.Log("Charging end");
        Destroy(_chargeUp);
        Instantiate(_projectilePrefab, _palmSpawnPosition.position, _palmSpawnPosition.rotation);
    }
}
