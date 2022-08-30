using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHandlePointerEvent
{
    GameObject gameObject { get; }

    void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo);
    void OnHoverEnd(IUIPointer Sender);

    void OnGripStart(IUIPointer Sender, RaycastHit RayInfo);
    void OnGripEnd(IUIPointer Sender);

    void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo);
    void OnTriggerEnd(IUIPointer Sender);
}