using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureTemplates", menuName = "ScriptableObjects/GestureTemplates", order = 1)]
public class GestureTemplates : ScriptableObject
{
    public List<TemplateData> Templates;
}
