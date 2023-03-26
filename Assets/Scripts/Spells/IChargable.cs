using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChargable
{
    public ICharger.ChargeInfo ChargeInfo { get; set; }
}
