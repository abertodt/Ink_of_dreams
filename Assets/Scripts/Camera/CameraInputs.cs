using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class CameraInputs : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PostProcessVolume _drawModeVolume;

    private bool _isDrawModeActive;
    
}
