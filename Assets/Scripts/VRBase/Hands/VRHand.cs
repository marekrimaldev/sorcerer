using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHand : MonoBehaviour
{
    [Tooltip("Whether to calculate physic collision")]
    [SerializeField] private bool _usePhysicHands = true;

    private void Awake()
    {
        EnablePhysics(_usePhysicHands);
    }

    private void EnablePhysics(bool value)
    {
        Collider[] cols = GetComponentsInChildren<Collider>();
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].enabled = value;
        }
    }
}
