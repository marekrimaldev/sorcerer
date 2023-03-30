using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classes implementing this interface can be traversed
/// Each ITraverzable defines its 4 successors in 4 directions LEFT, RIGHT, UP, DOWN
/// When traverzing ITraverzable OnHoverStart() and OnHoverStop() methods are called
/// </summary>
public interface ITraversable : ISelectable
{
    ITraversable LeftSuccessor { get; set; }
    ITraversable RightSuccessor { get; set; }
    ITraversable UpSuccessor { get; set; }
    ITraversable DownSuccessor { get; set; }

    void OnHoverStart();
    void OnHoverStop();
}
