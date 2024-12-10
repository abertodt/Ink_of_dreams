using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _gameEvent;
    [SerializeField] private CustomGameEvent _response;

    private void OnEnable()
    {
        _gameEvent.RegisterListerner(this);
    }

    private void OnDisable()
    {
        _gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        _response?.Invoke(sender, data);
    }
}
