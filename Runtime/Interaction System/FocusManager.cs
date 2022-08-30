using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FocusManager
{
    public static IFocusItem CurrentFocusItem { get; private set; }

    public static void SetFocus(IFocusItem Item)
    {
        if (Item != CurrentFocusItem) ClearFocus();

        CurrentFocusItem = Item;
        CurrentFocusItem.OnFocusStart();
    }

    public static void ClearFocus()
    {
        if (CurrentFocusItem != null) CurrentFocusItem.OnFocusEnd();
        CurrentFocusItem = null;
    }
}

public interface IFocusItem
{
    GameObject gameObject { get; }
    Transform transform { get; }

    void OnFocusStart();
    void OnFocusEnd();
}
