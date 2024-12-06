using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private List<PausableMonoBehaviour> pausableObjects = new List<PausableMonoBehaviour>();

    private bool isGamePaused = false;

    public bool IsGamePaused
    {
        get => isGamePaused;
        set
        {
            isGamePaused = value;
            NotifyPauseStateChange(isGamePaused);
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Register a pausable object
    public void RegisterPausableObject(PausableMonoBehaviour obj)
    {
        if (!pausableObjects.Contains(obj))
        {
            pausableObjects.Add(obj);
        }
    }

    // Unregister a pausable object
    public void UnregisterPausableObject(PausableMonoBehaviour obj)
    {
        if (pausableObjects.Contains(obj))
        {
            pausableObjects.Remove(obj);
        }
    }

    // Notify all registered objects about the pause state change
    private void NotifyPauseStateChange(bool paused)
    {
        foreach (var obj in pausableObjects)
        {
            obj.IsPaused = paused;
        }
    }

    // Example toggle pause function
    public void TogglePause()
    {
        IsGamePaused = !IsGamePaused;
    }
}