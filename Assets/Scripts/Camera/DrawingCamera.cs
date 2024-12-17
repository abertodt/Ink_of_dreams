using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCamera : MonoBehaviour
{
    [SerializeField] private GameObject _plane;
    [SerializeField] private Camera _camera;

    private void OnEnable()
    {
        GameManager.Instance.SecondaryCamera = _camera;
    }

    public void ApplyScreenshotToPlane(Component sender, object data)
    {
        if(sender is GameManager && data is Texture2D)
        {
            _plane.GetComponent<Renderer>().material.mainTexture = (Texture2D)data;
        }
    }
}
