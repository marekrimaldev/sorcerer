using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpell : Spell
{
    [SerializeField] private GameObject _laserSourceEffectPrefab;
    [SerializeField] private float _laserLength;
    
    private GameObject _laserSourceEffect;
    private GameObject _laserEndEffect;
    private LineRenderer[] _laserRenderers;

    protected override void Update()
    {
        base.Update();

        if(_laserRenderers != null &&_laserRenderers.Length > 0)
            UpdateLaser();
    }

    public override void Cast()
    {
        base.Cast();

        _laserRenderers = _prefabInstance.GetComponentsInChildren<LineRenderer>();

        _laserSourceEffect = Instantiate(_laserSourceEffectPrefab, _spawnPosition.position, _spawnPosition.rotation, transform);
        _laserEndEffect = Instantiate(_laserSourceEffectPrefab, _spawnPosition.position, _spawnPosition.rotation, transform);
    }

    public override void StopCast()
    {
        base.StopCast();

        for (int i = 0; i < _laserRenderers.Length; i++)
        {
            Destroy(_laserRenderers[i]);
        }
        _laserRenderers = null;

        Destroy(_laserSourceEffect);
        Destroy(_laserEndEffect);
    }

    private void UpdateLaser()
    {
        Vector3 laserEnd = _spawnPosition.position + _spawnPosition.forward * _laserLength;
        if(Physics.Linecast(_spawnPosition.position, laserEnd, out RaycastHit hitInfo))
        {
            laserEnd = hitInfo.point;

            _laserEndEffect.SetActive(true);
            _laserEndEffect.transform.position = laserEnd - _spawnPosition.forward * 0.05f;
        }
        else
        {
            _laserEndEffect.SetActive(false);
        }

        for (int i = 0; i < _laserRenderers.Length; i++)
        {
            _laserRenderers[i].SetPosition(0, _spawnPosition.position);
            _laserRenderers[i].SetPosition(1, laserEnd);
        }
    }
}
