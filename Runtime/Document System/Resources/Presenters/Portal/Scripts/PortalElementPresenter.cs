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
            subPresenter.Initialize(Title, Href);
        }
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

}
