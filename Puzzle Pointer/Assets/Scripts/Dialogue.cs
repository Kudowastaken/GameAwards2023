using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable InconsistentNaming

[System.Serializable]
public class Dialogue
{
    [SerializeField] private string name;
    public string Name { get { return name; }}
    
    [TextArea(3, 10)]
    public string[] sentences;
}
