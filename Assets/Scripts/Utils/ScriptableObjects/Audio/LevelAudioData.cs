using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelAudioData", menuName = "ScriptableObjects/LevelAudioData", order = 4)]
public class LevelAudioData : ScriptableObject
{
    [Header("Background Music")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public List<AudioClip> soundEffects;
}