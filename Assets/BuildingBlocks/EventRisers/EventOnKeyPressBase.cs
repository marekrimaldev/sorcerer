using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventOnKeyPressBase : MonoBehaviour
{
    [SerializeField] private KeyCode[] _keys;
    private List<KeyCode> _keysList = new List<KeyCode>();

    private void Awake()
    {
        for (int i = 0; i < _keys.Length; i++)
        {
            _keysList.Add(_keys[i]);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _keysList.Count; i++)
        {
            if (Input.GetKeyDown(_keysList[i]))
            {
                InvokeEvent();
            }
        }
    }

    protected abstract void InvokeEvent();
}
