using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuBuilder : MonoBehaviour, IMenuBuilder
{
    public abstract ITraversable[] GetTraverzables();
}
