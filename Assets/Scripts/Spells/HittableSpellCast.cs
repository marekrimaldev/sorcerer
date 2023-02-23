using UnityEngine;

public abstract class HittableSpellCast : Spell
{
    [SerializeField] protected OnHitEffectManager _onHitEffectManager;
}
