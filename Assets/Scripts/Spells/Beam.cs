using UnityEngine;
using UnityEngine.VFX;

public class Beam : HittableSpell
{
    [SerializeField] protected float _beamLength;
    [SerializeField] private BeamVFXController _beamVfxControllerPrefab;
    [SerializeField] private VFXController _sourceVFXControllerPrefab;
    [SerializeField] private VFXController _endVFXControllerPrefab;

    private Transform _parentTransform;

    private BeamVFXController _beamVfxController;
    private VFXController _sourceVFXController;
    private VFXController _endVFXController;

    private SphereCollider _laserEndTrigger;

    private bool _isActive = false;     // To prevend NullExp error

    private void Update()
    {
        if(_isActive)
            UpdateLaser();
    }

    private void OnTriggerStay(Collider other)
    {
        _onHitEffectManager.ApplyEffects(_endVFXController.transform.position, other.transform);
    }

    public override void Init(SpellCaster spell)
    {
        base.Init(spell);

        _isActive = true;

        _parentTransform = spell.AimIndicator.SpawnPoint;
        _beamVfxController = Instantiate(_beamVfxControllerPrefab, _parentTransform.position, _parentTransform.rotation, transform) as BeamVFXController;
        _sourceVFXController = Instantiate(_sourceVFXControllerPrefab, _parentTransform.position, _parentTransform.rotation, transform);
        _endVFXController = Instantiate(_endVFXControllerPrefab, _parentTransform.position, _parentTransform.rotation, transform);

        _laserEndTrigger = _endVFXController.gameObject.AddComponent<SphereCollider>();
        _laserEndTrigger.gameObject.layer = LayerMask.NameToLayer("IgnoreSpell");
        _laserEndTrigger.isTrigger = true;
        _laserEndTrigger.radius = 0.05f;
    }

    public override void Dispose()
    {
        base.Dispose();
        _isActive = false;
    }

    protected virtual Vector3 GetBeamEnd()
    {
        return _parentTransform.position + _parentTransform.forward * _beamLength;
    }

    protected void UpdateLaser()
    {
        Vector3 beamEnd = GetBeamEnd();
        if (Physics.Linecast(_parentTransform.position, beamEnd, out RaycastHit hitInfo, ~LayerMask.GetMask("IgnoreSpell")))
        {
            beamEnd = hitInfo.point;

            _endVFXController.EnableVFX(true);
            _endVFXController.transform.position = beamEnd - _parentTransform.forward * 0.02f;
        }
        else
        {
            _endVFXController.EnableVFX(false);
        }

        float currLength = Vector3.Distance(_parentTransform.position, beamEnd);
        _beamVfxController.SetBeamLength(currLength);
    }
}
