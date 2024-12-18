using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraFollow : PausableMonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform _camera;
    [SerializeField] private float _FOV;
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceToTarget;
    [SerializeField] private float _followHeight;
    [SerializeField] private float _followSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _minPitchRotation;
    [SerializeField] private float _maxPitchRotation;

    [Header("SphereCast")]
    [SerializeField] private float _sphereCastRadius;
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _smoothSpeed;

    [Header("Settings")]
    [SerializeField] private GameEvent _onScreenshotTaken;

    private float _yaw = 0f;
    private float _pitch = 0f;

    private void Start()
    {
        GetComponentInChildren<Camera>().fieldOfView = _FOV;
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
        transform.position = _target.position - _target.forward * _distanceToTarget + Vector3.up * _followHeight;
    }

    void OnEnable()
    {
        GameManager.Instance.OriginalCamera = GetComponentInChildren<Camera>();
    }

    protected override void OnPausableLateUpdate()
    {
        transform.position = _target.position;
        //Vector3 desiredPosition = _target.position - _target.forward * _distanceToTarget + Vector3.up * _followHeight;
        //transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
        //_camera.transform.LookAt(_target.position);

        _yaw += InputManager.Instance.CameraRotation.x * _rotationSpeed;
        _pitch -= InputManager.Instance.CameraRotation.y * _rotationSpeed;
        _pitch = Mathf.Clamp(_pitch, _minPitchRotation, _maxPitchRotation);



        transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        RaycastHit hit;

        Vector3 direction = _camera.transform.position - transform.position;
        direction.Normalize();

        if (Physics.SphereCast(transform.position, _sphereCastRadius, direction, out hit, _distance, _layerMask))
        {
            _camera.transform.localPosition = Vector3.back * hit.distance;
        }
        else
        {
            Vector3 targetPosition = transform.position - transform.forward * _distance;
            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, Time.deltaTime * _smoothSpeed);
        }

        //Vector3 directionToTarget = _target.position - _camera.position;
        //Quaternion lookAtRotation = Quaternion.LookRotation(directionToTarget);

        //Quaternion pitchRotation = Quaternion.Euler(_pitch, 0f, 0f);
        //_camera.localRotation = lookAtRotation * pitchRotation;
    }

    public void TakeScreenshot(Component sender, object data)
    {
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        _onScreenshotTaken?.Raise(this, screenshot);
    }
}
