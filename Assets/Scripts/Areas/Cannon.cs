using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private SpellCaster _spellCaster;
    [SerializeField] private float _minSecondsBetweenAttacks = 1f;
    [SerializeField] private float _maxSecondsBetweenAttacks = 5f;

    private void Start()
    {
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
