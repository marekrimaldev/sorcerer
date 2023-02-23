using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class GrabMove : Spell
{
    [SerializeField] private float _dashDistance = 3;
    [SerializeField] private float _dashDuration = .5f;

    private Transform _rigTransform;
    private Transform _handTransform;

    public override void Init(SpellCaster spell, float chargePercent = 1)
    {
        base.Init(spell, chargePercent);

        _rigTransform = GetComponentInParent<XROrigin>().transform;
        _handTransform = spell.AimIndicator.SpawnPoint;

        StartCoroutine(UpdateRigPositionCoroutine());
    }

    private IEnumerator UpdateRigPositionCoroutine()
    {
        Vector3 rigStartPos = _rigTransform.position;
        Vector3 handStartPos = rigStartPos - _handTransform.position;

        while (true)
        {
            Vector3 handPos = _rigTransform.position - _handTransform.position;
            Vector3 targetPos = rigStartPos + (handPos - handStartPos) * _dashDistance;

            _rigTransform.position = Vector3.Lerp(_rigTransform.position, targetPos, 0.1f);
            yield return null;


        }
    }

    //private IEnumerator DashCoroutine()
    //{
    //    Vector3 handEndPos = _handTransform.position;
    //    Vector3 dashDir = (_handStartPos - handEndPos).normalized;

    //    Transform rigTransform = GetComponentInParent<XROrigin>().transform;

    //    Vector3 rigStartPos = rigTransform.position;
    //    Vector3 rigDestinationPos = rigTransform.position + dashDir * _dashDistance;

    //    float t = 0;
    //    while (t <= 1)
    //    {
    //        rigTransform.position = Vector3.Lerp(rigStartPos, rigDestinationPos, t);
    //        t += Time.deltaTime / _dashDuration;

    //        yield return null;
    //    }
    //}
}
