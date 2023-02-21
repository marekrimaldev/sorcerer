using UnityEngine;

public class Beam : HittableSpellCast
{
    [SerializeField] private float _laserLength;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _laserSourceEffectPrefab;
    [SerializeField] private GameObject _laserEndEffectPrefab;

    private Transform _parentTransform;

    private LineRenderer[] _laserRenderers;
    private GameObject _laserInstance;
    private GameObject _laserSourceEffectInstance;
    private GameObject _laserEndEffectInstance;

    private SphereCollider _laserEndTrigger;

    private void Update()
    {
        if (_laserRenderers != null && _laserRenderers.Length > 0)
            UpdateLaser();
    }

    private void OnTriggerStay(Collider other)
    {
        _onHitEffectManager.ApplyEffects(_laserEndEffectInstance.transform.position, other.transform);
    }

    public override void Init(Spell spell, float chargePercent = 1)
    {
        base.Init(spell);

        _parentTransform = spell.AimIndicator.SpawnPoint;
        _laserInstance = Instantiate(_laserPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _laserRenderers = _laserInstance.GetComponentsInChildren<LineRenderer>();
        _laserSourceEffectInstance = Instantiate(_laserSourceEffectPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _laserEndEffectInstance = Instantiate(_laserEndEffectPrefab, _parentTransform.position, _parentTransform.rotation, transform);

        _laserEndTrigger = _laserEndEffectInstance.AddComponent<SphereCollider>();
        _laserEndEffectInstance.layer = LayerMask.NameToLayer("IgnoreSpell");
        _laserEndTrigger.isTrigger = true;
        _laserEndTrigger.radius = 0.05f;
    }

    private void UpdateLaser()
    {
        Vector3 laserEnd = _parentTransform.position + _parentTransform.forward * _laserLength;
        if (Physics.Linecast(_parentTransform.position, laserEnd, out RaycastHit hitInfo, ~LayerMask.GetMask("IgnoreSpell")))
        {
            laserEnd = hitInfo.point;

            _laserEndEffectInstance.SetActive(true);
            _laserEndEffectInstance.transform.position = laserEnd - _parentTransform.forward * 0.05f;
        }
        else
        {
            _laserEndEffectInstance.SetActive(false);
        }

        for (int i = 0; i < _laserRenderers.Length; i++)
        {
            _laserRenderers[i].SetPosition(0, _parentTransform.position);
            _laserRenderers[i].SetPosition(1, laserEnd);
        }
    }
}
