using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private SpellController _primarySpellPressPrefab;
    [SerializeField] private SpellController _primarySpellReleasePrefab;
    [SerializeField] private SpellController _secondarySpellPressPrefab;
    [SerializeField] private SpellController _secondarySpellReleasePrefab;

    private SpellController _primarySpellPressInstance;
    private SpellController _primarySpellReleaseInstance;
    private SpellController _secondarySpellPressInstance;
    private SpellController _secondarySpellReleaseInstance;

    private void Awake()
    {
        if(_primarySpellPressPrefab != null)
            _primarySpellPressInstance = Instantiate(_primarySpellPressPrefab, transform, false);

        if (_primarySpellReleasePrefab != null)
            _primarySpellReleaseInstance = Instantiate(_primarySpellReleasePrefab, transform, false);

        if (_secondarySpellPressPrefab != null)
            _secondarySpellPressInstance = Instantiate(_secondarySpellPressPrefab, transform, false);

        if (_secondarySpellReleasePrefab != null)
            _secondarySpellReleaseInstance = Instantiate(_secondarySpellReleasePrefab, transform, false);
    }

    public void PrimaryPress()
    {
        _primarySpellPressInstance?.StartCast();
    }

    public void PrimaryRelease()
    {

        _primarySpellPressInstance?.StopCast();
        _primarySpellReleaseInstance?.StartCast();
    }

    public void SecondaryPress()
    {
        _secondarySpellPressInstance?.StartCast();
    }

    public void SecondaryRelease()
    {
        _secondarySpellPressInstance?.StopCast();
        _secondarySpellReleaseInstance?.StartCast();
    }
}
