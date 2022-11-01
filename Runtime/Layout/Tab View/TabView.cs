using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabView : MonoBehaviour
{
    [SerializeField] private TabPanel DefaultPanel;

    public event Action SelectedTabChanged;

    private List<TabPanel> panels;

    public int SelectedTabIndex { get; private set; } = -1;

    private TabPanel activePanel;

    private void Start()
    {
        panels = transform.GetChildren().SelectNotNull(i => i.GetComponent<TabPanel>()).ToList();
        foreach(var panel in panels)
        {
            panel.ParentTabView = this;
            panel.Hide(true);
        }

        if (DefaultPanel != null)
            ShowTab(DefaultPanel);
        else
            SelectTabByIndex(0);
    }

    public void SelectTabByIndex(int TabIndex)
    {
        if (TabIndex == SelectedTabIndex) return;
        if (TabIndex >= panels.Count) return;

        if(activePanel != null)
        {
            activePanel.Hide();
        }

        SelectedTabIndex = TabIndex;
        activePanel = panels[SelectedTabIndex];
        activePanel.Show();
        SelectedTabChanged?.Invoke();
    }

    public void ShowTab(TabPanel Panel)
    {
        if (activePanel == Panel) return;
        if (!panels.Contains(Panel)) return;

        SelectTabByIndex(panels.IndexOf(Panel));
    }
}
