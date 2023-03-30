using UnityEngine.Events;

[System.Serializable]
public class VoidUnityEvent : UnityEvent<Void>
{
    public static readonly Void V;
}

public struct Void { }