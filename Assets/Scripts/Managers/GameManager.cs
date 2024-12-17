using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEvent _onCameraSwitch;
    [SerializeField] private GameEvent _onGameStateChange;

    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public GameState CurrentState { get; private set; } = GameState.Gameplay;

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

        _onGameStateChange?.Raise(this, newState);
    }
    

    public void TogglePause()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(GameState.Gameplay);
        }
        else
        {
            ChangeState(GameState.Paused);
        }
    }


    public void ToggleDrawMode()
    { 
        if (CurrentState == GameState.Gameplay)
        {
            ChangeState(GameState.Drawing);
        }
        else if(CurrentState == GameState.Drawing)
        {
            ChangeState(GameState.Gameplay);
        }
    }

    public void SwitchMainCamera(Component sender, object data)
    {
        OriginalCamera.enabled = !OriginalCamera.enabled;
        SecondaryCamera.enabled = !SecondaryCamera.enabled;

        if(sender is CameraFollow) _onCameraSwitch?.Raise(this, data);
    }
}

public enum GameState
{
    Gameplay,
    Paused,
    Drawing
}