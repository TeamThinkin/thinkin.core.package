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
        //try
        //{
            if(CurrentFocusItem != null && CurrentFocusItem.gameObject != null) CurrentFocusItem?.OnFocusEnd();
        //}
        //catch(System.Exception ex)
        //{
        //    Debug.LogError(ex);
        //}
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
