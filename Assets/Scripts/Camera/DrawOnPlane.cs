using UnityEngine;

public class DrawOnPlane : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Material drawingMaterial;

    private GameObject drawingPlane;
    private LineRenderer currentLine;
    private bool isDrawing = false;

    void Start()
    {
        SetupDrawingPlane();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartDrawing();
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            UpdateDrawing();
        }

        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            FinishDrawing();
        }
    }

    private void SetupDrawingPlane()
    {
        // Create and position the plane in front of the camera
        drawingPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        drawingPlane.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 2;
        drawingPlane.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

        // Apply a material with a RenderTexture
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mainCamera.targetTexture = renderTexture;

        drawingMaterial.mainTexture = renderTexture;
        drawingPlane.GetComponent<Renderer>().material = drawingMaterial;
    }

    private void StartDrawing()
    {
        isDrawing = true;
        currentLine = Instantiate(linePrefab).GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
    }

    private void UpdateDrawing()
    {
        // Get mouse position in world space and ensure it's on the plane's z-position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = drawingPlane.transform.position.z;

        // Update the LineRenderer with the new position
        currentLine.positionCount++;
        currentLine.SetPosition(currentLine.positionCount - 1, mouseWorldPosition);
    }

    private void FinishDrawing()
    {
        isDrawing = false;
        // Finish drawing (e.g., save line data, or prepare for new drawing)
    }
}