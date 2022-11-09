using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPointer : MonoBehaviour, IUIPointer
{
    [SerializeField] private LayerMask Layers;
    [SerializeField] private float MaxRayDistance = 10;
    [SerializeField] private LineRenderer Line;
    [SerializeField] private Transform HitPointIndicator;
    [SerializeField] private bool IsLineRendererEnabled = true;

    private IHandlePointerEvent hoverItem;
    private IHandlePointerEvent currentHoverItem;
    private IHandlePointerEvent currentTriggerItem;
    private IHandlePointerEvent currentGripItem;
    private RaycastHit rayInfo;
    private IUIPointerProvider provider;

    private void OnDestroy()
    {
        unsubscribeFromProvider();
    }

    public void SetProvider(IUIPointerProvider Provider)
    {
        unsubscribeFromProvider();

        this.provider = Provider;
        provider.PrimaryButtonStart += Provider_PrimaryButtonStart;
        provider.PrimaryButtonEnd += Provider_PrimaryButtonEnd;
        provider.SecondaryButtonStart += Provider_SecondaryButtonStart;
        provider.SecondaryButtonEnd += Provider_SecondaryButtonEnd;
    }

    private void unsubscribeFromProvider()
    {
        if (provider == null) return;
        provider.PrimaryButtonStart -= Provider_PrimaryButtonStart;
        provider.PrimaryButtonEnd -= Provider_PrimaryButtonEnd;
        provider.SecondaryButtonStart -= Provider_SecondaryButtonStart;
        provider.SecondaryButtonEnd -= Provider_SecondaryButtonEnd;
    }




    private void Update()
    {
        if (provider == null) return;

        if(Physics.Raycast(provider.GetRay(), out rayInfo, MaxRayDistance, Layers))
        {
            //PrimaryHand.AllowGrabbing = false;

            if (rayInfo.collider.gameObject.TryGetComponent<IHandlePointerEvent>(out hoverItem))
            {
                if (hoverItem != currentHoverItem)
                {
                    if (currentHoverItem != null)
                    {
                        notifyEndHover();
                        currentHoverItem = null;
                    }

                    currentHoverItem = hoverItem;
                    notifyHoverStart(rayInfo);
                }
            }
            else
            {
                currentHoverItem = null;
            }
        }
        else
        {
            //PrimaryHand.AllowGrabbing = true;

            if (currentHoverItem != null)
            {
                notifyEndHover();
                currentHoverItem = null;
            }
        }

        updateVisuals(rayInfo);
    }

    private void notifyHoverStart(RaycastHit rayInfo)
    {
        try
        {
            if(currentHoverItem != null) currentHoverItem.OnHoverStart(this, rayInfo);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error with UIPoint OnHoverStart: " + ex.Message);
        }
    }

    private void notifyEndHover()
    {
        try
        {
            if(currentHoverItem != null) currentHoverItem.OnHoverEnd(this);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error with UIPointer OnHoverEnd: " + ex.Message);
        }
    }

    private void updateVisuals(RaycastHit hitInfo)
    {
        if(!IsLineRendererEnabled)
        {
            Line.enabled = false;
            return;
        }

        if (hitInfo.collider != null)
        {
            Line.positionCount = 2;
            Line.SetPositions(new Vector3[] { transform.position, hitInfo.point });
            Line.enabled = true;

            HitPointIndicator.position = hitInfo.point;
            HitPointIndicator.rotation = Quaternion.FromToRotation(Vector3.forward, hitInfo.normal);
            HitPointIndicator.gameObject.SetActive(true);
        }
        else
        {
            Line.enabled = false;
            HitPointIndicator.gameObject.SetActive(false);
        }
    }

    private void Provider_PrimaryButtonStart()
    {
        if (currentHoverItem != null)
        {
            currentTriggerItem = currentHoverItem;
            currentTriggerItem.OnTriggerStart(this, rayInfo);
        }
    }

    private void Provider_PrimaryButtonEnd()
    {
        if (currentTriggerItem != null)
        {
            currentTriggerItem.OnTriggerEnd(this);
            currentTriggerItem = null;
        }
    }

    private void Provider_SecondaryButtonStart()
    {
        if (currentHoverItem != null)
        {
            currentGripItem = currentHoverItem;
            currentGripItem.OnGripStart(this, rayInfo);
        }
    }

    private void Provider_SecondaryButtonEnd()
    {
        if (currentGripItem != null)
        {
            currentGripItem.OnGripEnd(this);
            currentGripItem = null;
        }
    }
}
