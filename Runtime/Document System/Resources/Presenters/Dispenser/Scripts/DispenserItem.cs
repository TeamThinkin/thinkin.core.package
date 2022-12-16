using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserItem : MonoBehaviour, IHandlePointerEvent
{
    public DispenserElementPresenter ParentDispenser;

    private DispenserElementPresenter.ItemInfo itemInfo;
    private AudioSource audioSource;
    private IGrabbable grabbable;
    private bool isGrabbed;
    private bool isLinked;
    private Vector3 initialGrabPosition;
    private const float pluckDistanceThreshold = 0.2f;
    private const float pluckDistanceThresholdSquared = pluckDistanceThreshold * pluckDistanceThreshold;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (grabbable != null)
        {
            grabbable.OnBeforeGrab -= Grabbable_OnBeforeGrab;
            grabbable.OnRelease -= Grabbable_OnRelease;
            grabbable = null;
        }
    }

    public void SetItemInfo(DispenserElementPresenter.ItemInfo ItemInfo)
    {
        this.itemInfo = ItemInfo;

        grabbable = GetComponent<IGrabbable>();
        grabbable.OnBeforeGrab += Grabbable_OnBeforeGrab;
        grabbable.OnRelease += Grabbable_OnRelease;
        isLinked = true;
    }

    private void Grabbable_OnBeforeGrab(IGrabber Grabber, IGrabbable Grabbable)
    {
        audioSource.PlayOneShot(ParentDispenser.ItemGrabSound, 1f);
        itemInfo.IsDisconnected = true;
        itemInfo.Body.isKinematic = false;
        isGrabbed = true;
        initialGrabPosition = transform.position;
        ParentDispenser.DisableGestureZone();
    }

    private void Grabbable_OnRelease(IGrabber Grabber, IGrabbable Grabbable)
    {
        isGrabbed = false;
        ParentDispenser.EnableGestureZone();

        if (isLinked) //The item is still linked and has not been plucked from the dispenser
        {
            audioSource.PlayOneShot(ParentDispenser.ItemRestoreSound);
            ParentDispenser.ResetItem(itemInfo);
        }
        else
        {
            Destroy(this);
        }
    }


    private void Update()
    {
        if(isGrabbed && isLinked && (transform.position - initialGrabPosition).sqrMagnitude > pluckDistanceThresholdSquared)
        {
            onItemPlucked();
        }
    }

    private void onItemPlucked() //This is when uesr grabs an item and moves far enough away from the original location to consider it actually removed (plucked) from the dispenser
    {
        audioSource.PlayOneShot(ParentDispenser.ItemReleaseSound);
        isLinked = false;
        itemInfo.Line.gameObject.SetActive(false);

        transform.SetParent(ParentDispenser.GetRootParent().transform);
        gameObject.name = ParentDispenser.gameObject.name + ": " + itemInfo.Prefab.name + " " + ParentDispenser.GetNextItemId(); //NOTE: is important that the name of potentially networked items be unique
        ParentDispenser.ReplaceItem(itemInfo);
        ParentDispenser.FireOnItemDispensed(this.gameObject, itemInfo);
    }

    #region -- Unused IHandlePointerEvent's --
    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnGripEnd(IUIPointer Sender)
    {
    }

    public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnHoverEnd(IUIPointer Sender)
    {
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnTriggerEnd(IUIPointer Sender)
    {
    }
    #endregion
}
