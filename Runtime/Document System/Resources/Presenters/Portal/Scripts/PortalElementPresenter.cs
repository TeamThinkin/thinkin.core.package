using AngleSharp.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("portal", "Presenters/Portal/Portal", false)]
public class PortalElementPresenter : ElementPresenterBase
{
    public enum DisplayTypeEnum
    {
        Default,
        Door,
        PreviewOrb,
        Hidden
    }

    [SerializeField] private GameObject[] DisplayPrefabs; //NOTE: the order of this array is expected to match the order of values in the DisplayTypeEnum

    private bool isLocalDestination;
    private Vector3? localDestinationPosition;
    private Vector3? localDestinationEuler;
    private PortalSubPresenter subPresenter;

    public string Title { get; private set; }
    public string Href { get; private set; }
    public DisplayTypeEnum DisplayType { get; private set; }
    public PlacementInfo Placement { get; private set; }

    public override void ParseDataElement(IElement ElementData)
    {
        Title = ElementData.Attributes["title"].Value;
        Placement = GetPlacementInfo(ElementData);
        DisplayType = GetEnumFromAttribute<DisplayTypeEnum>(ElementData.Attributes["type"]);
        
        //Href = TransformRelativeUrlToAbsolute(ElementData.Attributes["href"].Value, ElementData);
        Href = ElementData.Attributes["href"].Value.TransformRelativeUrlToAbsolute(ElementData);

        ApplyPlacement(Placement, transform);
    }    

    public override async Task Initialize()
    {
        await base.Initialize();

        int displayIndex = (int)DisplayType - 1;
        if (displayIndex == -1) displayIndex = 0;
        if (displayIndex < DisplayPrefabs.Length) subPresenter = Instantiate(DisplayPrefabs[displayIndex])?.GetComponent<PortalSubPresenter>();

        if(subPresenter != null)
        {
            subPresenter.transform.SetParent(this.transform);
            subPresenter.transform.Reset();
            subPresenter.Initialize(this);
        }

        parseHref();
    }

    public override void ExecuteLayout()
    {
        base.ExecuteLayout();
        var subBounds = subPresenter.GetBoundingBox();
        if(subBounds.HasValue)
        {
            var v = subBounds.Value;
            v.center += subPresenter.transform.localPosition;

            var size = v.size;
            size.Scale(subPresenter.transform.localScale);
            size.Scale(transform.localScale);
            v.size = size;
            BoundingBox = v;
        }
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
                        if (i == 0)
                            localDestinationPosition = chunk.ToVector3(); // parseVector3String(chunk);
                        else if (i == 1)
                            localDestinationEuler = chunk.ToVector3(); // parseVector3String(chunk);
                    }
                    else break;
                }
                else break;
            } 

            if (localDestinationPosition.HasValue) Debug.Log("Destination position: " + localDestinationPosition.Value);
            if (localDestinationEuler.HasValue) Debug.Log("Destination euler: " + localDestinationEuler.Value);
        }
    }

    public async Task ActivatePortal()
    {
        if (isLocalDestination)
        {
            Debug.Log("Local destination");
            if (localDestinationEuler.HasValue) AppControllerBase.Instance.SetPlayerRotation(Quaternion.Euler(localDestinationEuler.Value));
            if (localDestinationPosition.HasValue) AppControllerBase.Instance.SetPlayerPosition(localDestinationPosition.Value);
        }
        else
        {
            await DestinationPresenter.Instance.DisplayUrl(Href);
        }
    }
}
