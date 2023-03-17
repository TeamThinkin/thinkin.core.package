using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FocusManager
{
    public static event System.Action<IFocusItem> OnFocusItemChanged;

    private static IFocusItem _currentFocusItem;
    public static IFocusItem CurrentFocusItem 
    { 
        get { return _currentFocusItem; }
        private set
        {
            if (value == _currentFocusItem) return;

            _currentFocusItem = value;
            OnFocusItemChanged?.Invoke(value);
        }
    }

    public static void SetFocus(IFocusItem Item)
    {
        if (Item != CurrentFocusItem) ClearFocus();

        CurrentFocusItem = Item;
        CurrentFocusItem?.OnFocusStart();
    }

    public static void ClearFocus()
    {
        CurrentFocusItem?.OnFocusEnd();
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
