using UnityEngine;

public class Gauntlet : MonoBehaviour
{
    [SerializeField] private SpellCaster _primarySpellCaster;
    [SerializeField] private SpellCaster _secondarySpellCaster;

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
}
