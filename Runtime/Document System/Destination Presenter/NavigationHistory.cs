using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavigationHistory
{
    private const int MaxHistoryItems = 3;

    public List<DocumentMetaInformation> Items { get; private set; } = new List<DocumentMetaInformation>();

    public int CurrentHistoryIndex { get; private set; }

    public bool CanNavigateBack => CurrentHistoryIndex > 0;

    public bool CanNavigateForward => CurrentHistoryIndex < Items.Count - 1;

    public async Task NavigateBack(Func<DocumentMetaInformation, Task> DisplayUrl)
    {
        if (!CanNavigateBack) return;

        CurrentHistoryIndex--;
        await DisplayUrl(Items[CurrentHistoryIndex]);
    }

    public async Task NavigateForward(Func<DocumentMetaInformation, Task> DisplayUrl)
    {
        if (!CanNavigateForward) return;

        CurrentHistoryIndex++;
        await DisplayUrl(Items[CurrentHistoryIndex]);
    }

    public void AddToHistory(DocumentMetaInformation docMeta)
    {
        if (CurrentHistoryIndex < Items.Count - 1) Items.RemoveRange(CurrentHistoryIndex + 1, Items.Count - CurrentHistoryIndex - 1);
        if (CurrentHistoryIndex + 1 == MaxHistoryItems) Items.RemoveRange(0, 1);

        Items.Add(docMeta);
        CurrentHistoryIndex = Items.Count - 1;
    }

    public void Dump()
    {
        Debug.Log("------- Nav History --------");
        Debug.Log("Index: " + CurrentHistoryIndex);

        int i = 0;
        foreach (var item in Items)
        {
            Debug.Log(i + ": " + item.Title);
            i++;
        }
    }
}
