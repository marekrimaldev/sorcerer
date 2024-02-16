using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BeamVFXParticles : BeamVFXController
{
    public override void SetBeamLength(float length)
    {
        _visualEffect.SetFloat("Length", length);
    }
}
