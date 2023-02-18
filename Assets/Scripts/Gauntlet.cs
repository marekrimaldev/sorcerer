using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private Spell _primarySpellPrefab;
    [SerializeField] private Spell _secondarySpelPrefab;

    private Spell _primarySpellInstance;
    private Spell _secondarySpellInstance;

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
