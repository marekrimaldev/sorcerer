using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private GameObject _prefab;
    [SerializeField] protected Transform _spawnPosition;
    [SerializeField] private LineRenderer _aimRenderer;
    [SerializeField] private float _aimLineLength = 15;
    [SerializeField] private bool _parentToGauntlet = true;
    [SerializeField] private bool _showAim = true;
    [SerializeField] private bool _destroyOnStopCast = true;

    protected GameObject _prefabInstance;

    protected virtual void Update()
    {
        if(_showAim)
            UpdateAimRenderer();
    }

    private void UpdateAimRenderer()
    {
        _aimRenderer.positionCount = 2;
        _aimRenderer.SetPosition(0, _spawnPosition.position);
        _aimRenderer.SetPosition(1, _spawnPosition.position + _spawnPosition.forward * _aimLineLength);
    }

    public virtual void Cast()
    {
        _prefabInstance = Instantiate(_prefab, _spawnPosition.position, _spawnPosition.rotation);
        _prefabInstance.transform.parent = _parentToGauntlet ? transform : null;
    }

    public virtual void StopCast()
    {
        if (_destroyOnStopCast) 
            Destroy(_prefabInstance);
    }
}
