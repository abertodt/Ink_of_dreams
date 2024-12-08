using System;
using UnityEngine;

public class PausableMonoBehaviour : MonoBehaviour
{
    public bool IsPaused { get; private set; }

    protected virtual void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    protected virtual void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    public virtual void OnGameStateChanged(GameState state)
    {
        IsPaused = (state == GameState.Paused || state == GameState.Drawing);
        OnPauseStateChanged(IsPaused);
    }

    protected virtual void OnPauseStateChanged(bool isPaused) { }

    protected virtual void Update()
    {
        if (IsPaused)
        {
            return;
        }

        OnPausableUpdate();
    }
    protected virtual void LateUpdate()
    {
        if (IsPaused)
        {
            return;
        }

        OnPausableLateUpdate();
    }

    protected virtual void OnPausableLateUpdate() {}

    protected virtual void OnPausableUpdate() { }
}
