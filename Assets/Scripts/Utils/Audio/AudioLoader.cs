using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    [Header("Level Audio Data")]
    [SerializeField] private LevelAudioData levelAudioData;

    private void OnEnable()
    {
        if (levelAudioData != null)
        {
            AudioManager.Instance.LoadAudioData(levelAudioData);
            AudioManager.Instance.PlayMusic();
        }
        else
        {
            Debug.LogWarning("LevelAudioData is not assigned in AudioLoader.");
        }
    }
}
