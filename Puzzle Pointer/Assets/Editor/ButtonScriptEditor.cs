using UnityEngine;
using UnityEditor;
using System.Reflection.Emit;
using static UnityEngine.GUILayout;

[CustomEditor(typeof(ButtonScript)), CanEditMultipleObjects]
public class ButtonScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ButtonScript buttonScript = (ButtonScript)target;

        Label("Button modes", EditorStyles.boldLabel);
        if (Button("Single Press Mode")) buttonScript.ButtonModeSingle();
        if (Button("Hold Mode")) buttonScript.ButtonModeHold();
    }
}
