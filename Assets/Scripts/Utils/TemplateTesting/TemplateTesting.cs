using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TemplateTesting : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GestureTemplates _gestureTemplates;
    [SerializeField] private LineGenerator _lineGenerator;
    [SerializeField] private TMP_Text _text;
    private GestureUtils _gestureUtils;

    private void Start()
    {
        _gestureUtils = new GestureUtils();
    }

    public void AddTemplate()
    {
        if (_inputField.text == "")
        {
            Debug.Log("Ponle un nombre puto");
            return;
        }

        if(_lineGenerator._currentLines.Count == 0)
        {
            Debug.Log("Dibuja algo perro");
            return;
        }


        List<GameObject> lines = _lineGenerator._currentLines;

        List<Vector2> points = _gestureUtils.FlattenStrokes(lines);

        points = _gestureUtils.NormalizeStroke(points);

        points = _gestureUtils.ResampleStroke(points);

        TemplateData data = new TemplateData(_inputField.text, points);

        _gestureTemplates.Templates.Add(data);
    }

    public void CompareTemplate()
    {

        List<GameObject> lines = _lineGenerator._currentLines;

        List<Vector2> points = _gestureUtils.FlattenStrokes(lines);

        TemplateData? closest = _gestureUtils.FindClosestTemplate(points, _gestureTemplates.Templates);

        Debug.Log(closest?.Name);
    }

    public void ResultText(string text)
    {
        _text.text = text;
    }
}
