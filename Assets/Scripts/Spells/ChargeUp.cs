using UnityEngine;

public class ChargeUp : SpellCast
{
    [SerializeField] private GameObject _effectPrefab;

    public override void Init(Spell spell)
    {
        base.Init(spell);

        Instantiate(_effectPrefab, transform.position, transform.rotation, transform);
    }
}
