using UnityEngine;

public abstract class HittableSpellCast : SpellCast
{
    [SerializeField] protected OnHitEffectManager _onHitEffectManager;
}
