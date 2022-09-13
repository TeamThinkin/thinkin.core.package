using AngleSharp.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("portal", "Presenters/Portal/Portal", false)]
public class PortalElementPresenter : ElementPresenterBase, IHandlePointerEvent
{
    public enum DisplayTypeEnum
    {
        Default,
        Hidden
    }

    [SerializeField] private TMPro.TMP_Text Label;
    [SerializeField] private Animator StateAnimator;
    [SerializeField] private GameObject DoorVisual;
    [SerializeField] private BoxCollider Collider;

    private bool isLocalDestination;
    private Vector3? localDestinationPosition;
    private Vector3? localDestinationEuler;

    public bool HasVisual { get; private set; }

    public string Title { get; private set; }
    public string Href { get; private set; }
    public DisplayTypeEnum DisplayType { get; private set; }
    public PlacementInfo Placement { get; private set; }

    public override void ParseDataElement(IElement ElementData)
    {
        Label.text = Title = ElementData.Attributes["title"].Value;
        Placement = GetPlacementInfo(ElementData);
        DisplayType = GetEnumFromAttribute<DisplayTypeEnum>(ElementData.Attributes["type"]);
        Href = TransformRelativeUrlToAbsolute(ElementData.Attributes["href"].Value, ElementData);

        ApplyPlacement(Placement, transform);
    }    

    public override async Task Initialize()
    {    
        HasVisual = true;

        switch (DisplayType)
        {
            case DisplayTypeEnum.Hidden:
                DoorVisual.SetActive(false);
                Collider.size = Vector3.one;
                Collider.center = Vector3.zero;
                break;
            default:
                break;
        }

        parseHref();
    }

    private void parseHref()
    {
        var hashIndex = Href.IndexOf("#");
        isLocalDestination = hashIndex > -1;
        if (isLocalDestination)
        {

            var hash = Href.Substring(hashIndex + 1);
            hash = Thinkin.Web.HttpUtility.UrlDecode(hash);
            // Assumed format: {0.1,0.2,0.3},{0.2,0.4,0.6}

            int startBrace = 0;
            int endBrace = 0;
            string chunk;
            for(int i=0;i<2;i++)
            {
                startBrace = hash.IndexOf('{', endBrace);

                if (startBrace > -1)
                {
                    endBrace = hash.IndexOf('}', startBrace);

                    if (endBrace > -1)
                    {
                        chunk = hash.Substring(startBrace, endBrace - startBrace + 1);
                        if(i == 0)
                            localDestinationPosition = parseVector3String(chunk);
                        else if(i == 1)
                            localDestinationEuler = parseVector3String(chunk);
                    }
                    else break;
                }
                else break;
            } 

            if (localDestinationPosition.HasValue) Debug.Log("Destination position: " + localDestinationPosition.Value);
            if (localDestinationEuler.HasValue) Debug.Log("Destination euler: " + localDestinationEuler.Value);
        }
    }

    private Vector3? parseVector3String(string text)
    {
        // Assumed format: {0.1,0.2,0.3}
        try
        {
            text = text.Trim();
            text = text.Substring(1, text.Length - 2);
            var pieces = text.Split(',');
            var x = float.Parse(pieces[0].Trim());
            var y = float.Parse(pieces[1].Trim());
            var z = float.Parse(pieces[2].Trim());
            return new Vector3(x, y, z);
        }
        catch(System.Exception ex)
        {
            Debug.LogError("Invalid vector string (Expecting {0.1,0.2,0.3}): " + text);
            return null;
        }
    }


    public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        StateAnimator.SetBool("Is Partially Open", true);
    }

    public void OnHoverEnd(IUIPointer Sender)
    {
        if(StateAnimator != null) StateAnimator.SetBool("Is Partially Open", false);
    }

    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnGripEnd(IUIPointer Sender)
    {
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        if (isLocalDestination)
        {
            Debug.Log("Local destination");
            if (localDestinationEuler.HasValue) AppControllerBase.Instance.SetPlayerRotation(Quaternion.Euler(localDestinationEuler.Value));
            if (localDestinationPosition.HasValue) AppControllerBase.Instance.SetPlayerPosition(localDestinationPosition.Value);
        }
        else
        {
            DestinationPresenter.Instance.DisplayUrl(Href);
        }
    }

    public void OnTriggerEnd(IUIPointer Sender)
    {
    }
}
