using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShield : Spell
{
    [SerializeField] private ShieldVFXController _shieldVFXControllerPrefab;
    
    private ShieldVFXController _shieldVFXController;

    public override void Init(SpellCaster spell, float chargePercent = 1)
    {
        base.Init(spell, chargePercent);

        _shieldVFXController = Instantiate(_shieldVFXControllerPrefab, spell.AimIndicator.SpawnPoint.position, spell.AimIndicator.SpawnPoint.rotation, transform);
        StartCoroutine(UpdateShieldDirection(spell.AimIndicator.SpawnPoint));
    }

    private IEnumerator UpdateShieldDirection(Transform directionTransform)
    {
        while (this)
        {
            _shieldVFXController.SetShieldDirection(directionTransform.forward);
            DiscartZRotation();

            yield return null;
        }
    }

    private void DiscartZRotation()
    {
        Vector3 eulers = _shieldVFXController.transform.rotation.eulerAngles;
        eulers.z = 0;
        _shieldVFXController.transform.rotation = Quaternion.Euler(eulers);
    }

    public override void Dispose()
    {
        _shieldVFXController.DestroyVFXImmediate();

        base.Dispose();
    }
}
