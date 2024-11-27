using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureUtils
{
    private int _numPoints = 64;
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
        // Use the class's private numPoints variable
        if (points == null || points.Count < 2 || _numPoints < 2)
        {
            Debug.LogError("Invalid input: Gesture must have at least two points, and numPoints must be at least 2.");
            return points;
        }

        // Calculate the total path length of the gesture
        float totalLength = 0f;
        List<float> distances = new List<float>
    {
        0f // Distance from the first point to itself is 0
    };

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

        // Ensure the last point is included
        if (resampledPoints.Count < _numPoints)
        {
            resampledPoints.Add(points[points.Count - 1]);
        }

        return resampledPoints;
    }

    public struct Template
    {
        public List<Vector2> Sequence;
        public float Distance;

        public Template(List<Vector2> sequence, float distance)
        {
            Sequence = sequence;
            Distance = distance;
        }
    }

    // Calculate DTW distance between two sequences
    public static float CalculateDTW(List<Vector2> sequenceA, List<Vector2> sequenceB)
    {
        int n = sequenceA.Count;
        int m = sequenceB.Count;

        // Create a 2D array for DTW distances
        float[,] dtw = new float[n + 1, m + 1];

        // Initialize with infinity
        for (int i = 0; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                dtw[i, j] = float.PositiveInfinity;
            }
        }

        // Set the starting point
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

    // Find the template with the closest resemblance
    public TemplateData FindClosestTemplate(List<Vector2> input, List<TemplateData> templates)
    {
        TemplateData closestTemplate = new TemplateData("", null);
        float closestDistance = float.PositiveInfinity;

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
}
