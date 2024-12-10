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
    [SerializeField] private PostProcessVolume _drawModeVolume;

    private float _yaw = 0f;
    private float _pitch = 0f;

    private void Start()
    {
        GetComponentInChildren<Camera>().fieldOfView = _FOV;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
        transform.position = _target.position - _target.forward * _distanceToTarget + Vector3.up * _followHeight;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.OriginalCamera = GetComponentInChildren<Camera>();
    }

    protected override void OnPausableLateUpdate()
    {
        Vector3 desiredPosition = _target.position - _target.forward * _distanceToTarget + Vector3.up * _followHeight;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, _followSpeed * Time.deltaTime);
        _camera.transform.LookAt(_target.position);

        //_yaw += InputManager.Instance.CameraRotation.x * _rotationSpeed;
        //_pitch -= InputManager.Instance.CameraRotation.y * _rotationSpeed;
        //_pitch = Mathf.Clamp(_pitch, _minPitchRotation, _maxPitchRotation);



        //transform.rotation = Quaternion.Euler(0f, _yaw, 0f);

        //Vector3 directionToTarget = _target.position - _camera.position;
        //Quaternion lookAtRotation = Quaternion.LookRotation(directionToTarget);

        //Quaternion pitchRotation = Quaternion.Euler(_pitch, 0f, 0f);
        //_camera.localRotation = lookAtRotation * pitchRotation;
    }

    public void ToggleDrawModeVolume()
    {
        if(_drawModeVolume.weight <= 0)
        {
            _drawModeVolume.weight = 1;
        }
        else
        {
            _drawModeVolume.weight = 0;
        }
    }
}
