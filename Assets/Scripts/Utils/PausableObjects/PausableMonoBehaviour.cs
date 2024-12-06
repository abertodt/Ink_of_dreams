using UnityEngine;

public class PausableMonoBehaviour : MonoBehaviour
{
    public bool IsPaused { get; private set; }

    protected virtual void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPausableObject(this);
            Debug.Log($"{name} registered with GameManager.");
        }
    }

    protected virtual void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterPausableObject(this);
        }
    }

    public virtual void OnGameStateChanged(GameState state)
    {
        IsPaused = (state == GameState.Paused || state == GameState.Drawing);
        OnPauseStateChanged(IsPaused);
    }

    protected virtual void OnPauseStateChanged(bool isPaused) { }

    public void SetPausedState(bool isPaused)
    {
        IsPaused = isPaused;
        OnPauseStateChanged(IsPaused);
    }

    protected virtual void Update()
    {
        if (IsPaused)
        {
            return;
        }

        OnPausableUpdate();
    }

    protected virtual void OnPausableUpdate() { }
}
