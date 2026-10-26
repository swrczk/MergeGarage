using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> _listeners = new();

    // Raise event through different method signatures
    // ############################################################

    public void Raise()
    {
        Raise(null, null);
    }

    public void Raise(object data)
    {
        Raise(null, data);
    }

    public void Raise(Component sender)
    {
        Raise(sender, null);
    }

    public void Raise(Component sender, object data)
    {
        for (var i = _listeners.Count - 1; i >= 0; i--) _listeners[i].OnEventRaised(sender, data);
    }

    // Manage Listeners
    // ############################################################

    public void RegisterListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
            _listeners.Remove(listener);
    }
}