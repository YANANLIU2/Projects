using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelsScriptableObject))]
[CanEditMultipleObjects]
public class LevelsScriptableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelsScriptableObject myScript = (LevelsScriptableObject)target;

        // Reset
        if (GUILayout.Button("Reset"))
        {
            myScript.Reset();
        }
    }
}
