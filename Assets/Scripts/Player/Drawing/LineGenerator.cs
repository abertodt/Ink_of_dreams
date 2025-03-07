using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject _linePrefab;
    [SerializeField] private int _maxActiveLines;
    [SerializeField] private float _minLineDistance;
    [SerializeField] private GestureTemplates _templates;
    [SerializeField] private TemplateTesting _templateTesting =  null;

    [Header("Events")]
    [SerializeField] private GameEvent _onDrawMatch;
    [SerializeField] private GameEvent _onDrawModeEnd;

    private Coroutine _drawing;
    public List<GameObject> _currentLines = new List<GameObject>();
    public int _activeLines;
    private float totalDistance = 0f;
    private GameObject line;
    private bool _isDrawing;
    private bool _isErasing;
    private GestureUtils _gestureUtils = new GestureUtils();


    //Hay un bug si borras la lista e intentas dibujar muy rapido otra vez

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Paused) return;
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
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.ChangeState(GameState.Drawing);
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

        List<Vector2> points = _gestureUtils.FlattenStrokes(_currentLines);

        TemplateData? foundTemplate = _gestureUtils.FindClosestTemplate(points, _templates.Templates);

        if (foundTemplate != null)
        {
            Debug.Log($"Event raised, {foundTemplate?.Name}");
            _templateTesting?.ResultText(foundTemplate?.Name);
            _onDrawMatch?.Raise(this, foundTemplate?.ObjectToSpawn);
            _onDrawModeEnd?.Raise(this, null);
            EraseLines();
        }
        else if (_activeLines >= _maxActiveLines)
        {
            EraseLines();
        }

    }

    private void EraseLines()
    {
        if (_isErasing) return;

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
        lastPosition.z = 5;

        

        bool lineStarted = false;


        while (true)
        {
            Vector3 mousepos = Input.mousePosition;
            mousepos.z = 1;
            Vector3 position = Camera.main.ScreenToWorldPoint(mousepos);
            position.z = 5;

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
}
