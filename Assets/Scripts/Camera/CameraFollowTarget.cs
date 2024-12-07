using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _lagSpeed;

    private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - transform.parent.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPosition = transform.parent.position + _offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _lagSpeed * Time.deltaTime);
    }
}