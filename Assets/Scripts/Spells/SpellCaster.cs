using UnityEngine;

[RequireComponent(typeof(IChargable))]
public class SpellCaster : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Spell _startCastSpellPrefab;
    [SerializeField] private Spell _stopCastSpellPrefab;
    [SerializeField] protected AimIndicator _aimIndicatorPrefab;

    protected Spell _startCastSpell;
    protected Spell _stopCastSpell;
    protected AimIndicator _aimIndicator;
    public IChargable Gauntlet { get; set; }

    public AimIndicator AimIndicator => _aimIndicator;

    private void Awake()
    {
        _aimIndicator = Instantiate(_aimIndicatorPrefab, transform.position, transform.rotation, transform);
        Gauntlet = GetComponent<IChargable>();
    }

    public virtual void StartCast()
    {
        if (_startCastSpellPrefab != null)
        {
            _startCastSpell = Instantiate(_startCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
            _startCastSpell.Init(this);
        }
    }

    public virtual void StopCast()
    {
        ICharger charger = _startCastSpell?.GetComponent<ICharger>();
        if (charger != null && Gauntlet != null)
            Gauntlet.ChargeInfo = charger.GetChargeInfo();

        _startCastSpell?.Dispose();

        if (_stopCastSpellPrefab != null)
        {
            _stopCastSpell = Instantiate(_stopCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
            _stopCastSpell.Init(this);
        }
    }
}
