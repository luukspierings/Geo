using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NatureSpawner))]
public class NatureSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var  myScript = (NatureSpawner)target;
        if (GUILayout.Button("Respawn"))
        {
            myScript.Clear();
            myScript.Generate();
        }
        if (GUILayout.Button("Clear"))
            myScript.Clear();

    }
}
