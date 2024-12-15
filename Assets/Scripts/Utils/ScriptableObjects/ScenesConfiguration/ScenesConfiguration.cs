using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneConfig", menuName = "ScriptableObjects/SceneConfig", order = 3)]
public class ScenesConfiguration : ScriptableObject
{
    [Tooltip("Names of scenes where Gameplay input should be enabled by default")]
    public List<string> GameplayScenes = new List<string>();

    [Tooltip("Names of scenes where UI input should be enabled by default")]
    public List<string> UIScenes = new List<string>();
}
