using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private List<PausableMonoBehaviour> _pausableObjects = new List<PausableMonoBehaviour>();

    public GameState CurrentState { get; private set; } = GameState.Normal;

    public event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        NotifyPauseStateChange(newState == GameState.Paused || newState == GameState.Drawing);
    }

    public void RegisterPausableObject(PausableMonoBehaviour obj)
    {
        if (!_pausableObjects.Contains(obj))
        {
            _pausableObjects.Add(obj);
            obj.OnGameStateChanged(CurrentState);
        }
    }

    public void UnregisterPausableObject(PausableMonoBehaviour obj)
    {
        if (_pausableObjects.Contains(obj))
        {
            _pausableObjects.Remove(obj);
        }
    }

    private void NotifyPauseStateChange(bool isPaused)
    {
        foreach (var obj in _pausableObjects)
        {
            obj.SetPausedState(isPaused);
        }
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Normal);
        }
        else
        {
            ChangeState(GameState.Paused);
        }
    }

    public void ToggleDrawMode()
    { 
        if (CurrentState == GameState.Normal)
        {
            ChangeState(GameState.Drawing);
        }
        else if(CurrentState == GameState.Drawing)
        {
            ChangeState(GameState.Normal);
        }
    }
}

public enum GameState
{
    Normal,
    Paused,
    Drawing
}