using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Settings")]
    [SerializeField] private AudioMixerGroup _mainMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private int _sfxPoolSize = 10;

    [Header("Pooling")]
    private List<AudioSource> _sfxSourcePool;

    private Dictionary<string, AudioClip> _audioClipDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _audioClipDictionary = new Dictionary<string, AudioClip>();
            InitializeMusicSource();
            InitializeSFXPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMusicSource()
    {
        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            if (_mainMixer != null) _musicSource.outputAudioMixerGroup = _mainMixer;
        }
    }

    private void InitializeSFXPool()
    {
        _sfxSourcePool = new List<AudioSource>();
        for (int i = 0; i < _sfxPoolSize; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
            if (_mainMixer != null) sfxSource.outputAudioMixerGroup = _mainMixer;
            _sfxSourcePool.Add(sfxSource);
        }
    }

    public void LoadAudioData(LevelAudioData levelAudioData)
    {
        _audioClipDictionary.Clear();

        if (levelAudioData.backgroundMusic != null)
        {
            _audioClipDictionary["BackgroundMusic"] = levelAudioData.backgroundMusic;
        }

        foreach (var clip in levelAudioData.soundEffects)
        {
            if (!_audioClipDictionary.ContainsKey(clip.name))
            {
                _audioClipDictionary[clip.name] = clip;
            }
        }
    }

    public void PlayMusic()
    {
        if (_audioClipDictionary.TryGetValue("BackgroundMusic", out var clip))
        {
            if (_musicSource.isPlaying && _musicSource.clip == clip) return;

            _musicSource.clip = clip;
            _musicSource.loop = true;
            _musicSource.Play();
        }
        else
        {
            Debug.LogWarning("No background music found for this level.");
        }
    }


    public void PlaySFX(string clipName, float volume = 1f, float pitch = 1f)
    {
        if (_audioClipDictionary.TryGetValue(clipName, out var clip))
        {
            var availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.clip = clip;
                availableSource.volume = volume;
                availableSource.pitch = pitch;
                availableSource.Play();
            }
            else
            {
                Debug.LogWarning("No available AudioSource in SFX pool.");
            }
        }
        else
        {
            Debug.LogWarning($"SFX clip '{clipName}' not found.");
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in _sfxSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null;
    }
}
