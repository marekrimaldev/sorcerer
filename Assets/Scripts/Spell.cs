using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour, ISpell
{
    [SerializeField] protected OnHitEffectManager _onHitEffectManager;

    public abstract void Init(SpellController spellController);
    public abstract void Dispose();
}
