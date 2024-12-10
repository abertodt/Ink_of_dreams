using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/GameEvent", order = 0)]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> Listeners = new List<GameEventListener>();

    public void Raise(Component sender, object data)
    {
        for(int i = 0; i < Listeners.Count; i++) 
        {
            Listeners[i].OnEventRaised(sender, data);
        }
    }

    public void RegisterListerner(GameEventListener listener)
    {
        if(!Listeners.Contains(listener)) 
        {
            Listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (Listeners.Contains(listener))
        {
            Listeners.Remove(listener);
        }
    }
}
