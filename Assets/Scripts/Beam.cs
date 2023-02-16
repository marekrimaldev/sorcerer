using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{
    [SerializeField] private float _laserLength;
    [SerializeField] private LineRenderer[] _laserRenderers;
    [SerializeField] private GameObject _laserSourceEffectPrefab;
    [SerializeField] private GameObject _laserEndEffectPrefab;

    private Transform _parentTransform;

    private GameObject _laserSourceEffect;
    private GameObject _laserEndEffect;

    private void Update()
    {
        if (_laserRenderers != null && _laserRenderers.Length > 0)
            UpdateLaser();
    }

    public override void Init(SpellController spellController)
    {
        _parentTransform = spellController.AimIndicator.SpawnPoint;
        transform.SetParent(_parentTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        _laserSourceEffect = Instantiate(_laserSourceEffectPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _laserEndEffect = Instantiate(_laserEndEffectPrefab, _parentTransform.position, _parentTransform.rotation, transform);
    }

    public override void Dispose()
    {
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
        Vector3 laserEnd = _parentTransform.position + _parentTransform.forward * _laserLength;
        if (Physics.Linecast(_parentTransform.position, laserEnd, out RaycastHit hitInfo))
        {
            laserEnd = hitInfo.point;

            _laserEndEffect.SetActive(true);
            _laserEndEffect.transform.position = laserEnd - _parentTransform.forward * 0.05f;

            _onHitEffectManager.ApplyEffects(hitInfo.point, hitInfo.transform);
        }
        else
        {
            _laserEndEffect.SetActive(false);
        }

        for (int i = 0; i < _laserRenderers.Length; i++)
        {
            _laserRenderers[i].SetPosition(0, _parentTransform.position);
            _laserRenderers[i].SetPosition(1, laserEnd);
        }
    }
}
