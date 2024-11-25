using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GestureTemplates", menuName = "ScriptableObjects/Utils/GestureTemplates", order = 0)]
public class GestureTemplates : ScriptableObject
{
    public List<TemplateData> Templates;
}
