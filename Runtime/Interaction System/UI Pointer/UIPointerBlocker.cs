using System.Collections;
using UnityEngine;

/// <summary>
/// This class can be placed on UI elements to have them swallow UI Pointer events, so that the pointer knows not clear the currently focused item
/// </summary>
public class UIPointerBlocker : MonoBehaviour, IHandlePointerEvent
{
    public void OnGripEnd(IUIPointer Sender)
    {
    }

    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnHoverEnd(IUIPointer Sender)
    {
    }

    public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnTriggerEnd(IUIPointer Sender)
    {
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }
}