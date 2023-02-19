using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private SpellCast _startCastSpellPrefab;
    [SerializeField] private SpellCast _stopCastSpellPrefab;
    [SerializeField] protected AimIndicator _aimIndicatorPrefab;

    protected SpellCast _startCastSpellInstance;
    protected SpellCast _stopCastSpellInstance;
    protected AimIndicator _aimIndicatorInstance;

    public AimIndicator AimIndicator => _aimIndicatorInstance;

    private void Awake()
    {
        _aimIndicatorInstance = Instantiate(_aimIndicatorPrefab, transform.position, transform.rotation, transform);
    }

    public virtual void StartCast()
    {
        if (_startCastSpellPrefab != null)
        {
            _startCastSpellInstance = Instantiate(_startCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
            _startCastSpellInstance.Init(this);
        }
    }

    public virtual void StopCast()
    {
        float chargePercent = 1;
        ICharger charger = _startCastSpellInstance.GetComponent<ICharger>();
        if (charger != null)
            chargePercent = charger.GetChargePercent();

        _startCastSpellInstance?.Dispose();

        if (_stopCastSpellPrefab != null)
        {
            _stopCastSpellInstance = Instantiate(_stopCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
            _stopCastSpellInstance.Init(this, chargePercent);
        }
    }
}
