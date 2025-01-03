﻿using AngleSharp.Dom;
using AngleSharp.XPath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public abstract class ElementPresenterBase : MonoBehaviour, IElementPresenter, ILayoutItem
{
    [SerializeField] private GameObject _SceneChildContainer;

    public IElementPresenter DOMParent { get; private set; }

    protected List<IElementPresenter> children = new List<IElementPresenter>();
    public IEnumerable<IElementPresenter> DOMChildren => children;

    public GameObject SceneChildrenContainer => _SceneChildContainer;

    public virtual Bounds? BoundingBox { get; protected set; }

    public virtual ILayoutContainerInfo LayoutContainerInfo { get; protected set; }

    public abstract void ParseDataElement(IElement ElementData);

    protected virtual void Awake()
    {
        if (transform.parent != null)
        {
            DOMParent = transform.parent.GetComponentInParent<IElementPresenter>(); //NOTE: GetComponentInParent's name is a misnomer and will also match THIS object
        }
    }

    public void AddDOMChild(IElementPresenter Child)
    {
        children.Add(Child);
        Child.SetDOMParent(this);
    }

    public void AddDOMChildren(IEnumerable<IElementPresenter> Children)
    {
        foreach(var child in Children) AddDOMChild(child);
    }

    public IElementPresenter GetRootParent()
    {
        if (DOMParent == null) 
            return this;
        else
            return DOMParent.GetRootParent();
    }

    public virtual async Task Initialize() { }

    public virtual void ExecuteLayout() 
    {
        LayoutContainerInfo = DOMParent?.LayoutContainerInfo;
        BoundingBox = DOMParent?.BoundingBox;
        foreach (var layoutItem in LayoutChildren)
        {
            layoutItem.ExecuteLayout();
        }
    }

    protected IEnumerable<ILayoutItem> LayoutChildren
    {
        get
        {
            return SceneChildrenContainer.transform.GetChildren().Select(i => i.GetComponent<ILayoutItem>()).WhereNotNull();
        }
    }

    public void RemoveDOMChild(IElementPresenter Child)
    {
        children.Remove(Child);
        Child.SetDOMParent(null);
    }

    public void RemoveDOMChildren(IEnumerable<IElementPresenter> Children)
    {
        foreach(var child in Children)
        {
            RemoveDOMChild(child);
        }
    }

    public void SetDOMParent(IElementPresenter ParentElement)
    {
        DOMParent = ParentElement;
    }


    public static void ApplyPlacement(IElement PresenterDataElement, Transform TargetSceneItem, Vector3? DefaultScale = null)
    {
        var placement = GetPlacementInfo(PresenterDataElement);
        ApplyPlacement(placement, TargetSceneItem, DefaultScale);
    }

    public static void ApplyPlacement(PlacementInfo Placement, Transform TargetSceneItem, Vector3? DefaultScale = null)
    { 
        ApplyPlacementPosition(Placement, TargetSceneItem);
        ApplyPlacementRotation(Placement, TargetSceneItem);
        ApplyPlacementScale(Placement, TargetSceneItem, DefaultScale);
    }

    public static void ApplyPlacementPosition(PlacementInfo Placement, Transform TargetSceneItem)
    {
        if (Placement.Position.HasValue) TargetSceneItem.localPosition = Placement.Position.Value;
    }

    public static void ApplyPlacementRotation(PlacementInfo Placement, Transform TargetSceneItem)
    {
        if (Placement.Rotation.HasValue) TargetSceneItem.localRotation = Placement.Rotation.Value;
    }

    public static void ApplyPlacementScale(PlacementInfo Placement, Transform TargetSceneItem, Vector3? DefaultScale = null)
    {
        if (!DefaultScale.HasValue) DefaultScale = Vector3.one;

        if (Placement.Size.HasValue && !Placement.Scale.HasValue)
            TargetSceneItem.localScale = Placement.Size.Value * DefaultScale.Value;
        else if (Placement.Scale.HasValue && !Placement.Size.HasValue)
            TargetSceneItem.localScale = Placement.AdjustedScale();
        else
            TargetSceneItem.localScale = DefaultScale.Value;
    }


    public static PlacementInfo GetPlacementInfo(IElement PresenterDataElement)
    {
        if (PresenterDataElement == null) return new PlacementInfo();

        var placementInfo = new PlacementInfo();

        placementInfo.Position = PresenterDataElement.GetAttribute("position").ToVector3();
        if(!placementInfo.Position.HasValue) placementInfo.Position = GetVectorFromElement(PresenterDataElement.SelectSingleNode("placement/position") as IElement);

        placementInfo.Rotation = PresenterDataElement.GetAttribute("rotation").ToQuaternion();
        if(!placementInfo.Rotation.HasValue) placementInfo.Rotation = GetQuaternionFromElement(PresenterDataElement.SelectSingleNode("placement/rotation") as IElement);

        placementInfo.Scale = PresenterDataElement.GetAttribute("scale").ToVector3();
        if(!placementInfo.Scale.HasValue) placementInfo.Scale = GetVectorFromElement(PresenterDataElement.SelectSingleNode("placement/scale") as IElement);

        placementInfo.Size = PresenterDataElement.GetAttribute("size").ToFloat();
        if(!placementInfo.Size.HasValue) placementInfo.Size = GetFloatFromElement(PresenterDataElement.SelectSingleNode("placement/size") as IElement);
        return placementInfo;
    }

    public static Vector3? GetVectorFromElement(IElement element)
    {
        if (element == null) return null;

        float x = 0, y = 0, z = 0;
        if (element.HasAttribute("x")) float.TryParse(element.Attributes["x"].Value, out x);
        if (element.HasAttribute("y")) float.TryParse(element.Attributes["y"].Value, out y);
        if (element.HasAttribute("z")) float.TryParse(element.Attributes["z"].Value, out z);
        return new Vector3(x, y, z);
    }

    public static Quaternion? GetQuaternionFromElement(IElement element)
    {
        if (element == null) return null;

        float x = 0, y = 0, z = 0, w = 0;
        if (element.HasAttribute("x")) float.TryParse(element.Attributes["x"].Value, out x);
        if (element.HasAttribute("y")) float.TryParse(element.Attributes["y"].Value, out y);
        if (element.HasAttribute("z")) float.TryParse(element.Attributes["z"].Value, out z);
        if (element.HasAttribute("w")) float.TryParse(element.Attributes["w"].Value, out w);
        return new Quaternion(x, y, z, w);
    }

    public static float? GetFloatFromElement(IElement element)
    {
        if(element == null) return null;

        string s = element.InnerHtml;
        if (string.IsNullOrEmpty(s)) return null;
        float v;
        if (float.TryParse(s, out v))
            return v;
        else
            return null;
    }

    public static T GetEnumFromAttribute<T>(IAttr attribute) where T : struct
    {
        if(attribute == null) return default(T);

        T value;

        string text = attribute?.Value;
        text = text?.Replace("-", "");

        if (Enum.TryParse<T>(text, true, out value))
            return value;
        else
            return default(T);
    }

    public Bounds? GetBounds()
    {
        return BoundingBox;
    }
}

public struct PlacementInfo
{
    public Vector3? Position;
    public Quaternion? Rotation;
    public Vector3? Scale;
    public float? Size;

    public Vector3 AdjustedScale()
    {
        if (!Scale.HasValue) Scale = Vector3.one;
        if (!Size.HasValue) Size = 1;
        return Scale.Value * Size.Value;
    }
}