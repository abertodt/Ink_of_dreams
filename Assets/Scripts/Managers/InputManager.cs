using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private GameEvent _onDrawModeStarted;
    [SerializeField] private GameEvent _onDrawModeEnd;
    [SerializeField] private GameEvent _onPauseInput;
    [SerializeField] private GameEvent _onResumeInput;
    //Borrar despues, son los cheats
    [SerializeField] private GameEvent _onDrawCheat1;
    [SerializeField] private GameEvent _onDrawCheat2;
    [SerializeField] private GameEvent _onDrawCheat3;

    //Borrar despues, esto no deberia ir aqui, es para los cheats
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefab1;
    [SerializeField] private GameObject _prefab2;
    [SerializeField] private GameObject _prefab3;

    [Header("Config")]
    [SerializeField] private ScenesConfiguration _scenesConfiguration;

    public static InputManager Instance { get; private set; }

    private PlayerControlls _playerControlls;

    private InputActionMap _currentActionMap;

    public bool IsMoving { get; private set; }
    public Vector2 MovementInput { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsDrawModeInputPressed { get; private set; }
    public Vector2 CameraRotation { get; private set; }
    public bool IsPauseInputPressed { get; private set; }
    public bool IsContinuePressed { get; private set; }
    public bool IsToggleLegendPressed {  get; private set; }

    private void Awake()
    {
        //Debug.Log("Awake");
        if (Instance == null)
        {
            _playerControlls = new PlayerControlls();
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
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetDefaultActionMap();
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

        _playerControlls.Gameplay.PauseGame.performed += OnEscPressed;

        _playerControlls.DrawingMode.Return.performed += OnToggleDrawMode;

        _playerControlls.UI.Back.started += OnEscPressed;

        _playerControlls.UI.Continue.started += OnContinuePressed;
        _playerControlls.UI.Continue.canceled += OnContinuePressed;

        _playerControlls.DrawingMode.ToggleLegend.performed += OnToggleLegend;

        #region Cheats

        _playerControlls.Gameplay.SpawnObj1.performed += OnSpawnObjCheat;
        _playerControlls.Gameplay.SpawnObj2.performed += OnSpawnObjCheat;

        #endregion Cheats
    }

    private void OnToggleLegend(InputAction.CallbackContext context)
    {
        IsToggleLegendPressed = context.ReadValueAsButton();
    }

    private void OnContinuePressed(InputAction.CallbackContext context)
    {
        IsContinuePressed = context.ReadValueAsButton();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SetDefaultActionMap();
    }

    private void SetDefaultActionMap()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        if (_scenesConfiguration.UIScenes.Contains(activeScene))
        {
            SetActionMap(_playerControlls.UI);
        }
        else if(_scenesConfiguration.GameplayScenes.Contains(activeScene))
        {
            SetActionMap(_playerControlls.Gameplay);
        }
        else
        {
            Debug.Log($"{activeScene} is not configurated in SceneConfig");
        }
    }

    private void OnEscPressed(InputAction.CallbackContext context)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        if (!_scenesConfiguration.UIScenes.Contains(activeScene))
        {
            IsPauseInputPressed = !IsPauseInputPressed;
            if (IsPauseInputPressed)
            {
                _onPauseInput?.Raise(this, IsPauseInputPressed);
                SetActionMap(_playerControlls.UI);
            }
            else
            {
                _onResumeInput?.Raise(this, IsPauseInputPressed);
                SetActionMap(_playerControlls.Gameplay);
            }
        }
        
    }

    private void OnSpawnObjCheat(InputAction.CallbackContext context)
    {
        InputAction buttonPressed = context.action;

        switch (buttonPressed.name)
        {
            case "SpawnObj1":
                _onDrawCheat1.Raise(this, _prefab1);
                break;
            case "SpawnObj2":
                _onDrawCheat2.Raise(this, _prefab2);
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
        if (GameManager.Instance.CurrentState == GameState.Drawing)
        {
            SetActionMap(_playerControlls.DrawingMode);
            _onDrawModeStarted.Raise(this, _currentActionMap);
        }
        else
        {
            SetActionMap(_playerControlls.Gameplay);
            _onDrawModeEnd.Raise(this, _currentActionMap);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

        _playerControlls.UI.Back.started -= OnEscPressed;

        _playerControlls.UI.Continue.started -= OnContinuePressed;
        _playerControlls.UI.Continue.canceled -= OnContinuePressed;

        _playerControlls.DrawingMode.ToggleLegend.performed -= OnToggleLegend;

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

        //Debug.Log($"{newActionMap.name} entered");
    }

    //Borrar mas adelante para hacerlo correctamente
    public void ReturnToGameplay(Component sender, object data) 
    {
        if(sender is GameManager && data is GameState.Gameplay)
        {
            SetActionMap(_playerControlls.Gameplay);
        }
        
    }

    public void ToggleUIForTutorial(Component sender, object data)
    {
        if(sender is Tutorial)
        {
            if ((bool)data){
                SetActionMap(_playerControlls.UI);
            }
            else
            {
                SetActionMap(_playerControlls.Gameplay);
            }
        }
    }
}
