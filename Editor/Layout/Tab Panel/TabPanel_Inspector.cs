using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TabPanel), true)]
public class TabPanel_Inspector : Editor
{
    public VisualTreeAsset UiTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement inspector = new VisualElement();
        
        var button = new Button(ActivateButton_clicked);
        button.text = "Activate Panel";
        inspector.Add(button);

        var container = new IMGUIContainer();
        inspector.Add(container);
        InspectorElement.FillDefaultInspector(container, serializedObject, this);

        return inspector;
    }

    private void ActivateButton_clicked()
    {
        var panel = this.target as TabPanel;
        panel.gameObject.SetActive(true);

        foreach(Transform sibling in panel.transform.parent)
        {
            sibling.gameObject.SetActive(sibling.gameObject == panel.gameObject);
            EditorUtility.SetDirty(sibling.gameObject);
        }
    }
}
