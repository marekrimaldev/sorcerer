using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private Spell _primarySpellPressPrefab;
    [SerializeField] private Spell _primarySpellReleasePrefab;
    [SerializeField] private Spell _secondarySpellPressPrefab;
    [SerializeField] private Spell _secondarySpellReleasePrefab;

    private Spell _primarySpellPress;
    private Spell _primarySpellRelease;
    private Spell _secondarySpellPress;
    private Spell _secondarySpellRelease;

    private void Awake()
    {
        if(_primarySpellPressPrefab != null)
            _primarySpellPress = Instantiate(_primarySpellPressPrefab, Vector3.zero, Quaternion.identity, transform);

        if (_primarySpellReleasePrefab != null)
            _primarySpellRelease = Instantiate(_primarySpellReleasePrefab, Vector3.zero, Quaternion.identity, transform);

        if (_secondarySpellPressPrefab != null)
            _secondarySpellPress = Instantiate(_secondarySpellPressPrefab, Vector3.zero, Quaternion.identity, transform);

        if (_secondarySpellReleasePrefab != null)
            _secondarySpellRelease = Instantiate(_secondarySpellReleasePrefab, Vector3.zero, Quaternion.identity, transform);
    }

    public void PrimaryPress()
    {
        _primarySpellPress?.Cast();
    }

    public void PrimaryRelease()
    {

        _primarySpellPress?.StopCast();
        _primarySpellRelease?.Cast();
    }

    public void SecondaryPress()
    {
        _secondarySpellPress?.Cast();
    }

    public void SecondaryRelease()
    {
        _secondarySpellPress?.StopCast();
        _secondarySpellRelease?.Cast();
    }
}
