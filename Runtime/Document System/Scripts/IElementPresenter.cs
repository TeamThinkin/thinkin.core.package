using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IElementPresenter
{
    GameObject gameObject { get; }
    Transform transform { get; }

    IElementPresenter DOMParent { get; }
    IEnumerable<IElementPresenter> DOMChildren { get; }
    GameObject SceneChildrenContainer { get; }
    Bounds? BoundingBox { get; }
    ILayoutContainerInfo LayoutContainerInfo { get; }

    void AddDOMChild(IElementPresenter Child);
    void AddDOMChildren(IEnumerable<IElementPresenter> Children);
    void RemoveDOMChild(IElementPresenter Child);
    void RemoveDOMChildren(IEnumerable<IElementPresenter> Children);
    IElementPresenter GetRootParent();

    Task Initialize();
    void ExecuteLayout();
    void ParseDataElement(IElement ElementData);

    void SetDOMParent(IElementPresenter ParentElement);
}