using UnityEngine;

public abstract class Spell : MonoBehaviour, ISpell
{
    [SerializeField] private bool _parentToGauntlet = true;
    [SerializeField] private bool _destroySpellOnDispose = true;

    public virtual void Init(SpellCaster spell, float chargePercent = 1)
    {
        if (_parentToGauntlet)
            transform.SetParent(spell.AimIndicator.SpawnPoint);
    }

    public virtual void Dispose()
    {
        if (_destroySpellOnDispose)
            Destroy(this.gameObject);
    }
}
