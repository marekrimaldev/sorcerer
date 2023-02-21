using UnityEngine;

public class ChargeUp : SpellCast, ICharger
{
    [SerializeField] private GameObject _effectPrefab;
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

    public override void Init(Spell spell, float chargePercent = 1)
    {
        base.Init(spell);

        Instantiate(_effectPrefab, transform.position, transform.rotation, transform);

        _chargeBeginTime = Time.time;
    }
}
