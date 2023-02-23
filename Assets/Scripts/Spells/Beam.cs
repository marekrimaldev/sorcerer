using UnityEngine;
using UnityEngine.VFX;

public class Beam : HittableSpellCast
{
    [SerializeField] private float _beamLength;
    [SerializeField] private BeamVFXController _beamVfxControllerPrefab;
    [SerializeField] private VFXController _sourceVFXControllerPrefab;
    [SerializeField] private VFXController _endVFXControllerPrefab;

    private Transform _parentTransform;

    private LineRenderer[] _beamRenderers;
    private BeamVFXController _beamVfxController;
    private VFXController _sourceVFXController;
    private VFXController _endVFXController;

    private SphereCollider _laserEndTrigger;

    private void Update()
    {
        if (_beamRenderers != null && _beamRenderers.Length > 0)
            UpdateLaser();
    }

    private void OnTriggerStay(Collider other)
    {
        
        _onHitEffectManager.ApplyEffects(_endVFXController.transform.position, other.transform);
    }

    public override void Init(SpellCaster spell, float chargePercent = 1)
    {
        base.Init(spell);

        _parentTransform = spell.AimIndicator.SpawnPoint;
        _beamVfxController = Instantiate(_beamVfxControllerPrefab, _parentTransform.position, _parentTransform.rotation, transform) as BeamVFXController;
        _beamRenderers = _beamVfxController.gameObject.GetComponentsInChildren<LineRenderer>();
        _sourceVFXController = Instantiate(_sourceVFXControllerPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _endVFXController = Instantiate(_endVFXControllerPrefab, _parentTransform.position, _parentTransform.rotation, transform);

        _laserEndTrigger = _endVFXController.gameObject.AddComponent<SphereCollider>();
        _laserEndTrigger.gameObject.layer = LayerMask.NameToLayer("IgnoreSpell");
        _laserEndTrigger.isTrigger = true;
        _laserEndTrigger.radius = 0.05f;
    }

    private void UpdateLaser()
    {
        Vector3 laserEnd = _parentTransform.position + _parentTransform.forward * _beamLength;
        if (Physics.Linecast(_parentTransform.position, laserEnd, out RaycastHit hitInfo, ~LayerMask.GetMask("IgnoreSpell")))
        {
            laserEnd = hitInfo.point;

            _endVFXController.gameObject.SetActive(true);
            _endVFXController.transform.position = laserEnd - _parentTransform.forward * 0.05f;
        }
        else
        {
            _endVFXController.gameObject.SetActive(false);
        }

        for (int i = 0; i < _beamRenderers.Length; i++)
        {
            _beamRenderers[i].SetPosition(0, _parentTransform.position);
            _beamRenderers[i].SetPosition(1, laserEnd);
        }
    }
}
