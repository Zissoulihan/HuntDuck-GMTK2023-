using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent<T> : ScriptableObject
{
    protected event Action<T> _event;

    public virtual void Subscribe(Action<T> subscribingMethod)
    {
        _event += subscribingMethod;
    }
    public virtual void Unsubscribe(Action<T> subscribedMethod)
    {
        _event -= subscribedMethod;
    }
    public virtual void TriggerEvent(T eventData)
    {
        _event?.Invoke(eventData);
    }
}
