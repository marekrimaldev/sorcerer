using UnityEngine;

public class Gauntlet : MonoBehaviour, IChargable
{
    [SerializeField] private SpellCaster _primarySpellCaster;
    [SerializeField] private SpellCaster _secondarySpellCaster;
    [SerializeField] private SpellCaster _shieldSpellCaster;

    private ICharger.ChargeInfo _chargeInfo;
    public ICharger.ChargeInfo ChargeInfo {
        get => _chargeInfo;
        set => _chargeInfo = value;
    }

    private void Awake()
    {
        _chargeInfo = new ICharger.ChargeInfo(1, 1, new Vector3[0]);
    }

    public void PrimaryPress()
    {
        _primarySpellCaster?.StartCast();
    }

    public void PrimaryRelease()
    {
        _primarySpellCaster?.StopCast();
    }

    public void SecondaryPress()
    { 
        _secondarySpellCaster?.StartCast();
    }

    public void SecondaryRelease()
    {
        _secondarySpellCaster?.StopCast();
    }

    public void ShieldPress()
    {
        _shieldSpellCaster?.StartCast();
    }

    public void ShieldRelease()
    {
        _shieldSpellCaster?.StopCast();
    }
}
