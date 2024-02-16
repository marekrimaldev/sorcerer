using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Connects provided MenuItems as a sequence
/// </summary>
public class LinearMenuBuilder : MenuBuilder
{
    [SerializeField] private MenuItem[] _menuItems;

    public override ITraversable[] GetTraverzables()
    {
        AssignAdjacency();
        return _menuItems;
    }

    private void AssignAdjacency()
    {
        for (int i = 0; i < _menuItems.Length; i++)
        {
            _menuItems[i].LeftSuccessor = _menuItems[i];
            _menuItems[i].RightSuccessor = _menuItems[i];
            _menuItems[i].DownSuccessor = _menuItems[(i + 1) % _menuItems.Length];
            _menuItems[i].UpSuccessor = _menuItems[(i + _menuItems.Length - 1) % _menuItems.Length];
        }
    }
}
