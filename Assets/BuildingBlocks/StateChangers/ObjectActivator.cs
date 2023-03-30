using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] _objects;

    public void Enable(bool value)
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].SetActive(value);
        }
    }
}
