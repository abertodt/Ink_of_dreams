using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TemplateData
{
    public string Name;
    public List<Vector2> Positions;
    public GameObject ObjectToSpawn;

    public TemplateData(string name, List<Vector2> points)
    {
        Name = name;
        Positions = points;
        ObjectToSpawn = null;
    }
}
