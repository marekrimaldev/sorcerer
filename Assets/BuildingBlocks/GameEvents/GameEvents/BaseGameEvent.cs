using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEvent<T> : ScriptableObject
{
	private List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

	public void AddListener(IGameEventListener<T> listener)
	{
		if(!this.listeners.Contains(listener))
		{
			this.listeners.Add(listener);
		}
	}

	public void RemoveListener(IGameEventListener<T> listener)
	{
		if(this.listeners.Contains(listener))
		{
			this.listeners.Remove(listener);
		}
	}

	public void Raise(T t)
	{
		for(int i = this.listeners.Count - 1; i >= 0; i--)
		{
			this.listeners[i].OnEventRaised(t);
		}
	}
}
