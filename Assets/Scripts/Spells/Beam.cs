using UnityEngine;
using UnityEngine.VFX;

public class Beam : HittableSpellCast
{
    [SerializeField] private float _beamLength;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _beamSourceEffectPrefab;
    [SerializeField] private GameObject _beamEndEffectPrefab;
    [SerializeField] private VisualEffect _keke;

    private Transform _parentTransform;

    private LineRenderer[] _beamRenderers;
    private GameObject _beamInstance;
    private GameObject _beamSourceEffectInstance;
    private GameObject _beamEndEffectInstance;

    private SphereCollider _laserEndTrigger;

    private void Update()
    {
        if (_beamRenderers != null && _beamRenderers.Length > 0)
            UpdateLaser();
    }

    private void OnTriggerStay(Collider other)
    {
        
        _onHitEffectManager.ApplyEffects(_beamEndEffectInstance.transform.position, other.transform);
    }

    public override void Init(Spell spell, float chargePercent = 1)
    {
        base.Init(spell);

        _parentTransform = spell.AimIndicator.SpawnPoint;
        _beamInstance = Instantiate(_laserPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _beamRenderers = _beamInstance.GetComponentsInChildren<LineRenderer>();
        _beamSourceEffectInstance = Instantiate(_beamSourceEffectPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _beamEndEffectInstance = Instantiate(_beamEndEffectPrefab, _parentTransform.position, _parentTransform.rotation, transform);

        _laserEndTrigger = _beamEndEffectInstance.AddComponent<SphereCollider>();
        _beamEndEffectInstance.layer = LayerMask.NameToLayer("IgnoreSpell");
        _laserEndTrigger.isTrigger = true;
        _laserEndTrigger.radius = 0.05f;
    }

    private void UpdateLaser()
    {
        Vector3 laserEnd = _parentTransform.position + _parentTransform.forward * _beamLength;
        if (Physics.Linecast(_parentTransform.position, laserEnd, out RaycastHit hitInfo, ~LayerMask.GetMask("IgnoreSpell")))
        {
            laserEnd = hitInfo.point;

            _beamEndEffectInstance.SetActive(true);
            _beamEndEffectInstance.transform.position = laserEnd - _parentTransform.forward * 0.05f;
        }
        else
        {
            _beamEndEffectInstance.SetActive(false);
        }

        for (int i = 0; i < _beamRenderers.Length; i++)
        {
            _beamRenderers[i].SetPosition(0, _parentTransform.position);
            _beamRenderers[i].SetPosition(1, laserEnd);
        }
    }
}
