using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHandlerChild : MonoBehaviour, IHandlePointerEvent
{
    public IHandlePointerEvent parent;

    public static void Inject(IHandlePointerEvent Parent)
    {
        var colliders = Parent.gameObject.GetComponentsInChildren<Collider>();
        foreach(var collider in colliders)
        {
            var child = collider.gameObject.AddComponent<PointerHandlerChild>();
            child.parent = Parent;
        }
    }

    private void Start()
    {
        if (parent == null) parent = GetComponentInParent<IHandlePointerEvent>();

        if (parent == null)
        {
            Debug.Log("PointerHandlerChild destroying itself because it does not appear to have a parent IHandlePointer");
            Destroy(this);
        }
    }

    public void OnGripEnd(IUIPointer Sender)
    {
        parent.OnGripEnd(Sender);
    }

    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        parent.OnGripStart(Sender, RayInfo);
    }

    public void OnHoverEnd(IUIPointer Sender)
    {
        parent.OnHoverEnd(Sender);
    }

    public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        parent.OnHoverStart(Sender, RayInfo);
    }

    public void OnTriggerEnd(IUIPointer Sender)
    {
        parent.OnTriggerEnd(Sender);
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        parent.OnTriggerStart(Sender, RayInfo);
    }
}
