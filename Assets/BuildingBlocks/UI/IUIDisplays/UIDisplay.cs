using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIDisplay : MonoBehaviour, IUIDisplay
{
    public abstract void StartHover(bool isSelectAvailable);
    public abstract void StopHover(bool isSelectAvailable);
}
