using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEvent _OnDrawModeStarted;
    [SerializeField] private GameEvent _OnDrawModeEnd;
    //Borrar despues, son los cheats
    [SerializeField] private GameEvent _OnDrawCheat1;
    [SerializeField] private GameEvent _OnDrawCheat2;
    [SerializeField] private GameEvent _OnDrawCheat3;

    //Borrar despues, esto no deberia ir aqui, es para los cheats
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefab1;
    [SerializeField] private GameObject _prefab2;
    [SerializeField] private GameObject _prefab3;

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
        //Debug.Log("Awake");
        if (Instance == null)
        {
            _playerControlls = new PlayerControlls();
            _currentActionMap = _playerControlls.Gameplay;
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
            Destroy(gameObject);
        }

        
    }

    private void OnEnable()
    {
        //Debug.Log("Enable");
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

        #region Cheats

        _playerControlls.Gameplay.SpawnObj1.performed += OnSpawnObjCheat;
        _playerControlls.Gameplay.SpawnObj2.performed += OnSpawnObjCheat;

        #endregion Cheats
    }

    private void OnSpawnObjCheat(InputAction.CallbackContext context)
    {
        InputAction buttonPressed = context.action;
        
        switch (buttonPressed.name) 
        {
            case "SpawnObj1":
                _OnDrawCheat1.Raise(this, _prefab1);
                break;
            case "SpawnObj2":
                _OnDrawCheat2.Raise(this, _prefab2);
                break;
            default:
                break;

        }
    }

    private void OnPauseGamePerformed(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("MainMenu");
        //GameManager.Instance.ChangeState(GameState.Paused);
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
            _OnDrawModeStarted.Raise(this, _currentActionMap);
        }
        else
        {
            SetActionMap(_playerControlls.Gameplay);
            _OnDrawModeEnd.Raise(this, _currentActionMap);
        }
        
        
    }

    private void OnDisable()
    {
        //Debug.Log("Disable");
        #region MovementInput

        _playerControlls.Gameplay.Move.started -= OnMovementInput;
        _playerControlls.Gameplay.Move.canceled -= OnMovementInput;
        _playerControlls.Gameplay.Move.performed -= OnMovementInput;

        #endregion MovementInput

        #region RunInput

        _playerControlls.Gameplay.Run.started -= OnRunInput;
        _playerControlls.Gameplay.Run.canceled -= OnRunInput;
        _playerControlls.Gameplay.Run.performed -= OnRunInput;

        #endregion RunInput

        _playerControlls.Gameplay.ToggleDrawMode.performed -= OnToggleDrawMode;

        _playerControlls.Gameplay.CameraRotation.started -= OnCameraRotation;
        _playerControlls.Gameplay.CameraRotation.canceled -= OnCameraRotation;

        _playerControlls.Gameplay.PauseGame.performed -= OnPauseGamePerformed;

        _playerControlls.DrawingMode.Return.performed -= OnToggleDrawMode;

        #region Cheats

        _playerControlls.Gameplay.SpawnObj1.performed -= OnSpawnObjCheat;
        _playerControlls.Gameplay.SpawnObj2.performed -= OnSpawnObjCheat;

        #endregion Cheats

        _currentActionMap?.Disable();
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
