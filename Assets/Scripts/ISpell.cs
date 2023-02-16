using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    void Init(SpellController spellController);
    void Dispose();
}
