using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region SINGLETON

    private static Player _instance;
    public static Player Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField] private Transform _headTransform;
    [SerializeField] private Transform _rigTransform;
    [SerializeField] private Transform _lHandTransform;
    [SerializeField] private Transform _rHandTransform;

    public Transform Head => _headTransform;
    public Transform Rig => _rigTransform;
    public Transform LHand => _lHandTransform;
    public Transform RHand => _rHandTransform;
}
