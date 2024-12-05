using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class CameraInputs : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PostProcessVolume _drawModeVolume;

    private PlayerInput _playerInput;
    private bool _isDrawModeActive;
    

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _playerInput.CameraControlls.DrawMode.started += OnDrawModeInput;
    }

    private void OnDrawModeInput(InputAction.CallbackContext context)
    {
        _isDrawModeActive = !_isDrawModeActive;

        if(_isDrawModeActive) 
        {
            _drawModeVolume.weight = 1f;
            Time.timeScale = 0f;
        }
        else
        {
            _drawModeVolume.weight = 0f;
            Time.timeScale = 1f;
        }
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}
