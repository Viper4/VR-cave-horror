using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SaveObject))]
public class SaveObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate IDs"))
        {
            SaveObject[] saveObjects = FindObjectsOfType<SaveObject>();
            for(uint i = 0; i < saveObjects.Length; i++)
            {
                saveObjects[i].saveID = i;
                EditorUtility.SetDirty(saveObjects[i]);
            }
        }
    }
}
