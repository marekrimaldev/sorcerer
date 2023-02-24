using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Spell _startCastSpellPrefab;
    [SerializeField] private Spell _stopCastSpellPrefab;
    [SerializeField] protected AimIndicator _aimIndicatorPrefab;

    protected Spell _startCastSpell;
    protected Spell _stopCastSpell;
    protected AimIndicator _aimIndicator;

    public AimIndicator AimIndicator => _aimIndicator;

    private void Awake()
    {
        _aimIndicator = Instantiate(_aimIndicatorPrefab, transform.position, transform.rotation, transform);
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
        float chargePercent = 1;
        ICharger charger = _startCastSpell?.GetComponent<ICharger>();
        if (charger != null)
            chargePercent = charger.GetChargePercent();

        _startCastSpell?.Dispose();

        if (_stopCastSpellPrefab != null)
        {
            _stopCastSpell = Instantiate(_stopCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
            _stopCastSpell.Init(this, chargePercent);
        }
    }
}
