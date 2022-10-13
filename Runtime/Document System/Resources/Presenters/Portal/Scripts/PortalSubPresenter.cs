using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalSubPresenter : MonoBehaviour, IHandlePointerEvent
{
    protected PortalElementPresenter parentPortalPresenter;

    public virtual void Initialize(PortalElementPresenter ParentPortalPresenter)
    {
        this.parentPortalPresenter = ParentPortalPresenter;
    }

    public virtual Bounds? GetBoundingBox()
    {
        return null;
    }

    public virtual void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo) 
    {
        Debug.Log("Portal SubPresenter hover start");
    }

    public virtual void OnHoverEnd(IUIPointer Sender) { }

    public virtual void OnGripStart(IUIPointer Sender, RaycastHit RayInfo) { }

    public virtual void OnGripEnd(IUIPointer Sender) { }

    public virtual void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo) 
    {
        parentPortalPresenter?.ActivatePortal();
    }

    public virtual void OnTriggerEnd(IUIPointer Sender) { }
}
