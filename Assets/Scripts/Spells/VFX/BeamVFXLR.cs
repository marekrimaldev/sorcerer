using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamVFXLR : BeamVFXController
{
    [SerializeField] private LineRenderer[] _lineRenderers;

    public override void SetBeamLength(float length)
    {
        throw new System.NotImplementedException();
    }
}
