using UnityEngine;

public abstract class HittableSpell : Spell
{
    [SerializeField] protected OnHitEffectManager _onHitEffectManager;
}
