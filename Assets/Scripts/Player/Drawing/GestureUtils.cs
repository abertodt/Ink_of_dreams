using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureUtils
{
    private int _numPoints = 100;
    private float _threshold = 10;
    public List<Vector2> NormalizeStroke(List<Vector2> stroke)
    {
        if (stroke == null || stroke.Count == 0)
            return new List<Vector2>();

        // Calculate bounding box
        float minX = float.MaxValue, minY = float.MaxValue;
        float maxX = float.MinValue, maxY = float.MinValue;

        foreach (var point in stroke)
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
        foreach (var point in stroke)
        {
            Vector2 normalizedPoint = (point - center) / scale;
            normalizedPoints.Add(normalizedPoint);
        }

        return normalizedPoints;
    }

    public List<Vector2> FlattenStrokes(List<GameObject> strokes)
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

    public List<Vector2> ResampleStroke(List<Vector2> points)
    {
        if (points == null || points.Count < 2 || _numPoints < 2)
        {
            Debug.LogError("Invalid input: Gesture must have at least two points, and numPoints must be at least 2.");
            return points;
        }

        // Calculate the total path length of the gesture
        float totalLength = 0f;
        List<float> distances = new List<float> { 0f };

        for (int i = 1; i < points.Count; i++)
        {
            float dist = Vector2.Distance(points[i - 1], points[i]);
            totalLength += dist;
            distances.Add(totalLength); // Cumulative distance
        }

        // Calculate the interval length between resampled points
        float interval = totalLength / (_numPoints - 1);

        // Create the resampled points
        List<Vector2> resampledPoints = new List<Vector2>
    {
        points[0]
    };

        float currentDistance = interval;
        int currentIndex = 1;

        while (resampledPoints.Count < _numPoints)
        {
            // Walk along the original gesture until reaching the next interval
            while (currentIndex < points.Count && distances[currentIndex] < currentDistance)
            {
                currentIndex++;
            }

            if (currentIndex >= points.Count)
                break;

            // Linearly interpolate to find the exact position of the next resampled point
            Vector2 p1 = points[currentIndex - 1];
            Vector2 p2 = points[currentIndex];
            float t = (currentDistance - distances[currentIndex - 1]) / (distances[currentIndex] - distances[currentIndex - 1]);

            Vector2 interpolatedPoint = Vector2.Lerp(p1, p2, t);
            resampledPoints.Add(interpolatedPoint);

            currentDistance += interval;
        }

        if (resampledPoints.Count < _numPoints)
        {
            resampledPoints.Add(points[points.Count - 1]);
        }

        return resampledPoints;
    }


    public static float CalculateDTW(List<Vector2> sequenceA, List<Vector2> sequenceB)
    {
        int n = sequenceA.Count;
        int m = sequenceB.Count;

        float[,] dtw = new float[n + 1, m + 1];

        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                dtw[i, j] = float.PositiveInfinity;
            }
        }

        dtw[0, 0] = 0;

        // Compute DTW
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                float cost = Vector2.Distance(sequenceA[i - 1], sequenceB[j - 1]);
                dtw[i, j] = cost + Mathf.Min(dtw[i - 1, j],         // Insertion
                                             dtw[i, j - 1],         // Deletion
                                             dtw[i - 1, j - 1]);    // Match
            }
        }

        return dtw[n, m];
    }

    public TemplateData? FindClosestTemplate(List<Vector2> input, List<TemplateData> templates)
    {
        input = NormalizeStroke(input);
        input = ResampleStroke(input);
        float closestDistance = float.PositiveInfinity;

        TemplateData? closestTemplate = TryFindClosestTemplate(input, templates, ref closestDistance);

        if (closestTemplate == null)
        {
            List<Vector2> flippedInput = FlipStrokeHorizontally(input);
            closestTemplate = TryFindClosestTemplate(flippedInput, templates, ref closestDistance);
        }

        return closestDistance <= _threshold ? closestTemplate : null;
    }

    private TemplateData? TryFindClosestTemplate(List<Vector2> input, List<TemplateData> templates, ref float closestDistance)
    {
        TemplateData? closestTemplate = null;

        foreach (var template in templates)
        {
            float distance = CalculateDTW(input, template.Positions);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTemplate = template;
            }
        }

        return closestTemplate;
    }

    private List<Vector2> FlipStrokeHorizontally(List<Vector2> input)
    {
        List<Vector2> flippedInput = new List<Vector2>(input);
        for (int i = 0; i < flippedInput.Count; i++)
        {
            flippedInput[i] = new Vector2(-flippedInput[i].x, flippedInput[i].y);
        }
        return flippedInput;
    }
}
