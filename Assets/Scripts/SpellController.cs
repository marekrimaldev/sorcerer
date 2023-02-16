using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private Spell _spell;
    [SerializeField] private bool _destroySpellOnStopCast = true;
    [SerializeField] protected AimIndicator _aimIndicator;

    protected Spell _spellInstance;

    public AimIndicator AimIndicator => _aimIndicator;

    public virtual void StartCast()
    {
        _spellInstance = Instantiate(_spell, _aimIndicator.SpawnPoint.position, _aimIndicator.SpawnPoint.rotation);
        _spellInstance.transform.parent = transform;

        _spellInstance.Init(this);
    }

    public virtual void StopCast()
    {
        _spellInstance.Dispose();

        if (_destroySpellOnStopCast) 
            Destroy(_spellInstance);
    }
}
