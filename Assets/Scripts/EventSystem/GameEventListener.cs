using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CustomGameEvent : UnityEvent<Component, object>
{
}

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")] public GameEvent _gameEvent;

    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public CustomGameEvent _response;

    private void OnEnable()
    {
        _gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        _response.Invoke(sender, data);
    }
}