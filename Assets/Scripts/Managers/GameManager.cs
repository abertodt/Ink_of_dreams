using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public GameState CurrentState { get; private set; } = GameState.Normal;

    public event Action<GameState> OnGameStateChanged;


    public Camera OriginalCamera;
    public Camera SecondaryCamera;

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