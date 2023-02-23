using UnityEngine;
using UnityEngine.VFX;

public class ChargeUp : Spell, ICharger
{
    [SerializeField] private VFXController _chargeUpVFXControllerPrefab;
    [SerializeField] private float _maxChargeDuration;
    [SerializeField] private bool _parentRotation = false;

    private float _chargeBeginTime;

    private void Update()
    {
        if (!_parentRotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public float GetChargePercent()
    {
        float chargeTime = Time.time - _chargeBeginTime;
        return Mathf.Min(chargeTime / _maxChargeDuration, 1);
    }

    public override void Init(SpellCaster spell, float chargePercent = 1)
    {
        base.Init(spell);

        Instantiate(_chargeUpVFXControllerPrefab, transform.position, transform.rotation, transform);

        _chargeBeginTime = Time.time;
    }
}
