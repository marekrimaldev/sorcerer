using UnityEngine;
using UnityEngine.Events;

public abstract class BaseGameEventListener<T, E, UE> : MonoBehaviour, IGameEventListener<T> where E : BaseGameEvent<T> where UE : UnityEvent<T>
{
    public E GameEvent;

    public UE UnityEvent;

    void OnEnable()
    {
        this.GameEvent.AddListener(this);
    }

    void OnDisable()
    {
        this.GameEvent.RemoveListener(this);
    }

    public void OnEventRaised(T t)
    {
        if (this.UnityEvent != null)
        {
            UnityEvent.Invoke(t);
        }
    }

}