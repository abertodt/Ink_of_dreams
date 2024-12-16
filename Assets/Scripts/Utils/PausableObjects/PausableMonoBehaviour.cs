using System;
using UnityEngine;

public class PausableMonoBehaviour : MonoBehaviour
{
    public bool IsPaused { get; private set; }

    private void CheckGameState()
    {
        IsPaused = (GameManager.Instance.CurrentState == GameState.Paused || GameManager.Instance.CurrentState == GameState.Drawing);
        OnPauseStateChanged(IsPaused);
    }

    protected virtual void OnPauseStateChanged(bool isPaused) { }

    protected virtual void Update()
    {
        CheckGameState();

        if (IsPaused)
        {
            return;
        }

        OnPausableUpdate();
    }
    protected virtual void LateUpdate()
    {
        CheckGameState();

        if (IsPaused)
        {
            return;
        }

        OnPausableLateUpdate();
    }

    protected virtual void OnPausableLateUpdate() {}

    protected virtual void OnPausableUpdate() { }
}
