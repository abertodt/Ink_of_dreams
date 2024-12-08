using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEvent _DrawModePressed;

    public static InputManager Instance { get; private set; }

    private PlayerControlls _playerControlls;

    private InputActionMap _currentActionMap;

    public bool IsMoving { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsDrawModeInputPressed { get; private set; }
    public Vector2 CameraRotation { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _playerControlls = new PlayerControlls();
        _currentActionMap = _playerControlls.Gameplay;
    }

    private void OnEnable()
    {
        _currentActionMap?.Enable();
        #region MovementInput

        _playerControlls.Gameplay.Move.started += OnMovementInput;
        _playerControlls.Gameplay.Move.canceled += OnMovementInput;
        _playerControlls.Gameplay.Move.performed += OnMovementInput;

        #endregion MovementInput

        #region RunInput

        _playerControlls.Gameplay.Run.started += OnRunInput;
        _playerControlls.Gameplay.Run.canceled += OnRunInput;
        _playerControlls.Gameplay.Run.performed += OnRunInput;

        #endregion RunInput

        _playerControlls.Gameplay.ToggleDrawMode.performed += OnToggleDrawMode;

        _playerControlls.Gameplay.CameraRotation.started += OnCameraRotation;
        _playerControlls.Gameplay.CameraRotation.canceled += OnCameraRotation;

        _playerControlls.Gameplay.PauseGame.performed += OnPauseGamePerformed;

        _playerControlls.DrawingMode.Return.performed += OnToggleDrawMode;
    }

    private void OnPauseGamePerformed(InputAction.CallbackContext context)
    {
        GameManager.Instance.ChangeState(GameState.Paused);
    }

    private void OnCameraRotation(InputAction.CallbackContext context)
    {
        CameraRotation = context.ReadValue<Vector2>();
    }

    private void OnToggleDrawMode(InputAction.CallbackContext context)
    {
        GameManager.Instance.ToggleDrawMode();
        if(GameManager.Instance.CurrentState == GameState.Drawing)
        {
            SetActionMap(_playerControlls.DrawingMode);
        }
        else
        {
            SetActionMap(_playerControlls.Gameplay);
        }
        
        _DrawModePressed.Raise();
    }

    private void OnDisable()
    {
        _currentActionMap?.Disable();

        _playerControlls.Gameplay.Move.started -= OnMovementInput;
        _playerControlls.Gameplay.Move.canceled -= OnMovementInput;
        _playerControlls.Gameplay.Move.performed -= OnMovementInput;

        _playerControlls.Gameplay.Run.started -= OnRunInput;
        _playerControlls.Gameplay.Run.canceled -= OnRunInput;
        _playerControlls.Gameplay.Run.performed -= OnRunInput;

        _playerControlls.Gameplay.ToggleDrawMode.started -= OnToggleDrawMode;

        _playerControlls.Gameplay.CameraRotation.started -= OnCameraRotation;
        _playerControlls.Gameplay.CameraRotation.canceled -= OnCameraRotation;

        _playerControlls.DrawingMode.Return.performed -= OnToggleDrawMode;
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

    private void SetActionMap(InputActionMap newActionMap)
    {
        if (_currentActionMap != null && _currentActionMap != newActionMap)
        {
            _currentActionMap.Disable();
        }

        _currentActionMap = newActionMap;
        _currentActionMap?.Enable();

        Debug.Log($"{newActionMap.name} entered");
    }
}
