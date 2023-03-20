using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSubPresenter : PortalSubPresenter
{
    [SerializeField] private TMPro.TMP_Text Label;
    [SerializeField] private Animator StateAnimator;

    public override void Initialize(string Title, string Href)
    {
        base.Initialize(Title, Href);
        StateAnimator?.SetBool("Is Partially Open", false);
        Label.text = Title;
    }

    public override void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        base.OnHoverStart(Sender, RayInfo);
        Debug.Log("Door hover start");
        StateAnimator?.SetBool("Is Partially Open", true);
    }

    public override void OnHoverEnd(IUIPointer Sender)
    {
        base.OnHoverEnd(Sender);
        if(gameObject != null && StateAnimator != null) StateAnimator.SetBool("Is Partially Open", false);
    }
}
