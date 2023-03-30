using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIDisplay
{
    void StartHover(bool isSelectAvailable);
    void StopHover(bool isSelectAvailable);
}
