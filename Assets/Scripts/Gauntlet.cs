using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private SpellCaster _primarySpellPrefab;
    [SerializeField] private SpellCaster _secondarySpelPrefab;

    private SpellCaster _primarySpellInstance;
    private SpellCaster _secondarySpellInstance;

    private void Awake()
    {
        if(_primarySpellPrefab != null)
            _primarySpellInstance = Instantiate(_primarySpellPrefab, transform, false);

        if (_secondarySpelPrefab != null)
            _secondarySpellInstance = Instantiate(_secondarySpelPrefab, transform, false);
    }

    public void PrimaryPress()
    {
        _primarySpellInstance?.StartCast();
    }

    public void PrimaryRelease()
    {

        _primarySpellInstance?.StopCast();
    }

    public void SecondaryPress()
    {
        _secondarySpellInstance?.StartCast();
    }

    public void SecondaryRelease()
    {
        _secondarySpellInstance?.StopCast();
    }
}
