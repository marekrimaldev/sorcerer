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
        //Debug.Log("Transform position = " + transform.position);
        //Debug.Log("Transform local position = " + transform.localPosition);
        //Debug.Log("Aim indicator position = " + _aimIndicatorPrefab.SpawnPoint.position);
        //Debug.Log("Aim indicator local position = " + _aimIndicatorPrefab.SpawnPoint.localPosition);
    }

    public virtual void StartCast()
    {
        if (_startCastSpellPrefab != null)
            StartCastSpell();
    }

    private void StartCastSpell()
    {
        _startCastSpellInstance = Instantiate(_startCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
        _startCastSpellInstance.Init(this);
    }

    public virtual void StopCast()
    {
        if (_stopCastSpellPrefab != null)
            StopCastSpell();

        _startCastSpellInstance?.Dispose();
    }

    private void StopCastSpell()
    {
        _stopCastSpellInstance = Instantiate(_stopCastSpellPrefab, AimIndicator.SpawnPoint.position, AimIndicator.SpawnPoint.rotation);
        _stopCastSpellInstance.Init(this);
    }
}
