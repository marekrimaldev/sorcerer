using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is a set of connected ITraverzables
/// Use IMenuBuilder to get ITraverzables 
/// Each menu has its own IMenuTraverzator which can traverse the menu
/// </summary>
[RequireComponent(typeof(ITraversator))]
public class Menu : MonoBehaviour
{
    [SerializeField] private MenuBuilder _menuBuilder;
    private ITraversable[] _traverzables;

    private void Start()
    {
        _traverzables = _menuBuilder.GetTraverzables();
        Debug.Log("Setting traverzable = " + _traverzables[0]);
        GetComponent<ITraversator>().SetTraversable(_traverzables[0]);
    }

    private void OnDisable()
    {
        for (int i = 0; i < _traverzables.Length; i++)
        {
            _traverzables[i].OnHoverStop();
        }
    }
}
