using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ButtonInteractable), true)]
public class ButtonInteractable_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        var labelProp = serializedObject.FindProperty("Label");
        var label = labelProp.objectReferenceValue as TMPro.TMP_Text;
        if (label != null)
        {
            label.text = EditorGUILayout.TextField(label.text);
            label.ForceMeshUpdate();
        }
        base.OnInspectorGUI();
    }
}
