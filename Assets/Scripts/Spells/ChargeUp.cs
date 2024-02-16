using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ChargeUp : Spell, ICharger
{
    [SerializeField] private VFXController _chargeUpVFXControllerPrefab;
    [SerializeField] private float _maxChargeDuration;
    [SerializeField] private bool _parentRotation = false;

    private float _chargeBeginTime;
    private List<Vector3> _trajectory = new List<Vector3>();

    private const float SampleInterval = 0.05f;

    private void Update()
    {
        if (!_parentRotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public ICharger.ChargeInfo GetChargeInfo()
    {
        StopAllCoroutines();

        float chargeTime = Time.time - _chargeBeginTime;
        float percent = Mathf.Min(chargeTime / _maxChargeDuration, 1);

        //for (int i = 0; i < _trajectory.Count; i++)
        //{
        //    Debug.Log("p = " + _trajectory[i]);
        //}

        ICharger.ChargeInfo info = new ICharger.ChargeInfo(chargeTime, percent, _trajectory.ToArray());
        info.GetAcceleration();

        return info;
    }

    public override void Init(SpellCaster spell)
    {
        base.Init(spell);

        Instantiate(_chargeUpVFXControllerPrefab, transform.position, transform.rotation, transform);
        StartCoroutine(SamplingCoroutine());
    }

    private IEnumerator SamplingCoroutine()
    {
        _chargeBeginTime = Time.time;

        while (this)
        {
            _trajectory.Add(transform.position);
            yield return new WaitForSeconds(SampleInterval);
        }
    }
}
