using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private int _maxActiveLines;
    [SerializeField] private float _minLineDistance;

    Coroutine _drawing;
    public List<GameObject> _currentLines = new List<GameObject>();
    public List<Vector2> _combinedStroke;
    int _activeLines;
    float totalDistance = 0f;
    GameObject line;
    bool _isDrawing;
    bool _isErasing;


    //Hay un bug si borras la lista e intentas dibujar muy rapido otra vez

    // Update is called once per frame
    void Update()
    {
        if (_isErasing) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartLine();
        }

        if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }

        if (Input.GetMouseButtonDown(1))
        {
            EraseLines();
        }

        _combinedStroke = FlattenStrokes(_currentLines);
    }

    private void StartLine()
    {
        if (_isDrawing) return;

        _isDrawing = true;

        if(_activeLines >= _maxActiveLines)
        {
            line = null;
            return;
        }

        if (_drawing != null)
        {
            StopCoroutine(_drawing);
        }

        _drawing = StartCoroutine(DrawLine());

    }

    private void FinishLine()
    {
        if(!_isDrawing) return;

        _isDrawing = false;

        if(line != null)
        {
            if (totalDistance < _minLineDistance)
            {
                //Debug.Log(totalDistance);
                _currentLines.Remove(line);
                Destroy(line);
                _activeLines--;
            }
            else
            {
                _currentLines.Add(line);
            }
        }
        
        
        StopCoroutine(_drawing);

    }

    private void EraseLines()
    {
        if(_isErasing) return;

        _isErasing = true;

        List<GameObject> linesToErase = new List<GameObject>(_currentLines);

        foreach (GameObject line in linesToErase)
        {
            Destroy(line);
        }

        _currentLines.Clear();
        _activeLines = 0;

        _isErasing = false;
    }

    IEnumerator DrawLine()
    {
        line = Instantiate(_linePrefab, Vector3.zero, Quaternion.identity);
        _activeLines++;
        
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        Vector3 lastPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastPosition.z = 0;

        

        bool lineStarted = false;


        while (true)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            float distance = Vector2.Distance(position, lastPosition);

            if(distance > 0.01f)
            {
                if (!lineStarted)
                {
                    lineRenderer.positionCount = 1;
                    lineRenderer.SetPosition(0, lastPosition);
                    lineStarted = true;
                }
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
                lastPosition = position;
                totalDistance += distance;
            }


            yield return null;
        }

    }

    private List<Vector2> NormalizeStrokes(List<Vector2> strokes)
    {
        if (strokes == null || strokes.Count == 0)
            return new List<Vector2>();

        // Calculate bounding box
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var point in strokes)
        {
            minX = Mathf.Min(minX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxX = Mathf.Max(maxX, point.x);
            maxY = Mathf.Max(maxY, point.y);
        }

        // Calculate center and scale
        Vector2 center = new Vector2((minX + maxX) / 2, (minY + maxY) / 2);
        float scale = Mathf.Max(maxX - minX, maxY - minY);

        // Normalize points
        List<Vector2> normalizedPoints = new List<Vector2>();
        foreach (var point in strokes)
        {
            Vector2 normalizedPoint = (point - center) / scale;
            normalizedPoints.Add(normalizedPoint);
        }

        return normalizedPoints;
    }

    private List<Vector2> FlattenStrokes(List<GameObject> strokes)
    {
        List<Vector2> flattened = new List<Vector2>();
        foreach (var stroke in strokes)
        {
            LineRenderer lr = stroke.GetComponent<LineRenderer>();
            for (int i = 0; i < lr.positionCount; i++)
            {
                Vector3 point = lr.GetPosition(i);
                flattened.Add(new Vector2(point.x, point.y));
            }
        }
        return flattened;
    }

    //string RecognizeGesture(List<GameObject> inputStrokes)
    //{
    //    // Normalize input
    //    NormalizeStrokes(inputStrokes);

    //    // Flatten input strokes
    //    List<Vector2> inputPoints = FlattenStrokes(inputStrokes);

    //    string bestMatch = "Unrecognized";
    //    float bestDistance = float.MaxValue;

    //    foreach (var template in gestureTemplates)
    //    {
    //        List<Vector2> templatePoints = FlattenStrokes(template.Value); // Flatten the template strokes
    //        float distance = CalculateGestureDistance(inputPoints, templatePoints);

    //        if (distance < bestDistance)
    //        {
    //            bestDistance = distance;
    //            bestMatch = template.Key;
    //        }
    //    }

    //    return bestMatch;
    //}
}
