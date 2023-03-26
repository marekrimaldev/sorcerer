using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour, IChargable
{
    [SerializeField] private SpellCaster _spellCaster;
    [SerializeField] private float _minSecondsBetweenAttacks = 1f;
    [SerializeField] private float _maxSecondsBetweenAttacks = 5f;

    private ICharger.ChargeInfo _chargeInfo;
    public ICharger.ChargeInfo ChargeInfo
    {
        get => _chargeInfo;
        set => _chargeInfo = value;
    }

    private void Start()
    {
        _chargeInfo = new ICharger.ChargeInfo(1, 1, new Vector3[0]);

        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (this)
        {
            _spellCaster.StartCast();

            float delay = Random.Range(_minSecondsBetweenAttacks, _maxSecondsBetweenAttacks);
            yield return new WaitForSeconds(delay);

            _spellCaster.StopCast();
        }
    }
}
