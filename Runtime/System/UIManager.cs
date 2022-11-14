using System.Collections;
using UnityEngine;

public class UIManager : IUIManager
{
    public event System.Action<GameObject> OnMakeGrabbable;

    public void MakeGrabbable(GameObject Item)
    {
        OnMakeGrabbable?.Invoke(Item);
    }
}