using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IElementPresenter
{
    GameObject gameObject { get; }
    Transform transform { get; }

    //IElement ElementData { get; }
    IElementPresenter DOMParent { get; }
    IEnumerable<IElementPresenter> DOMChildren { get; }
    GameObject SceneChildrenContainer { get; }

    void AddDOMChild(IElementPresenter Child);
    void AddDOMChildren(IEnumerable<IElementPresenter> Children);
    void RemoveDOMChild(IElementPresenter Child);
    void RemoveDOMChildren(IEnumerable<IElementPresenter> Children);

    Task Initialize();
    void ParseDataElement(IElement ElementData);

    void SetDOMParent(IElementPresenter ParentElement);
}