using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInput _playerInput;

    public bool IsMoving { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsDrawModeInputPressed { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _playerInput.Enable();

        #region MovementInput

        _playerInput.Gameplay.Move.started += OnMovementInput;
        _playerInput.Gameplay.Move.canceled += OnMovementInput;
        _playerInput.Gameplay.Move.performed += OnMovementInput;

        #endregion MovementInput

        #region RunInput

        _playerInput.Gameplay.Run.started += OnRunInput;
        _playerInput.Gameplay.Run.canceled += OnRunInput;
        _playerInput.Gameplay.Run.performed += OnRunInput;

        #endregion RunInput

        _playerInput.Gameplay.ToggleDrawMode.performed += OnToggleDrawMode;
    }

    private void OnToggleDrawMode(InputAction.CallbackContext context)
    {
        if(context.performed) 
        {
            GameManager.Instance.ToggleDrawMode();
        }
    }

    private void OnDisable()
    {
        _playerInput.Disable();

        _playerInput.Gameplay.Move.started -= OnMovementInput;
        _playerInput.Gameplay.Move.canceled -= OnMovementInput;
        _playerInput.Gameplay.Move.performed -= OnMovementInput;

        _playerInput.Gameplay.Run.started -= OnRunInput;
        _playerInput.Gameplay.Run.canceled -= OnRunInput;
        _playerInput.Gameplay.Run.performed -= OnRunInput;

        _playerInput.Gameplay.ToggleDrawMode.started -= OnToggleDrawMode;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
        IsMoving = MovementInput != Vector2.zero;
    }

    private void OnRunInput(InputAction.CallbackContext context)
    {
        IsRunning = context.ReadValueAsButton();
    }
}
