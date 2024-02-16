using UnityEngine;

public abstract class Spell : MonoBehaviour, ISpell
{
    [SerializeField] private bool _parentToGauntlet = true;
    [SerializeField] private bool _destroySpellOnDispose = true;

    public IChargable Gauntlet => _spellCaster.Gauntlet;
    private SpellCaster _spellCaster;

    public virtual void Init(SpellCaster spellCaster)
    {
        _spellCaster = spellCaster;

        if (_parentToGauntlet)
            transform.SetParent(spellCaster.AimIndicator.SpawnPoint);
    }

    public virtual void Dispose()
    {
        if (_destroySpellOnDispose)
            Destroy(this.gameObject);
    }
}
