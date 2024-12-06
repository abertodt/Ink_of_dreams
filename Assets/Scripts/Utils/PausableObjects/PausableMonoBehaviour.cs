using UnityEngine;

public class PausableMonoBehaviour : MonoBehaviour
{
    private bool isPaused = false;

    public bool IsPaused
    {
        get => isPaused;
        set
        {
            isPaused = value;
            OnPauseStateChanged(isPaused);
        }
    }
    protected virtual void OnPauseStateChanged(bool paused) {}

    protected virtual void Update()
    {
        if (isPaused)
        {
            return;
        }

        OnPausableUpdate();
    }
    protected virtual void OnPausableUpdate() { }

    protected virtual void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPausableObject(this);
        }
    }

    protected virtual void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UnregisterPausableObject(this);
        }
    }
}
