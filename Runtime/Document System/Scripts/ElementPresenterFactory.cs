using AngleSharp.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

public static class ElementPresenterFactory
{
    public struct ElementPresenterInfo
    {
        public string HtmlTag;
        public Type PresenterType;
        public GameObject Prefab;
        public bool IsContainer;
    }

    public static Dictionary<string, ElementPresenterInfo> Presenters { get; private set; }

    static ElementPresenterFactory()
    {
        discoverTypes();
    }

    private static void discoverTypes()
    {
        Presenters = new Dictionary<string, ElementPresenterInfo>();

        var types = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where !t.IsAbstract && t.GetInterfaces().Contains(typeof(IElementPresenter))
                    select t;

        foreach (var type in types)
        {
            var presenterAttribute = type.GetCustomAttribute<ElementPresenterAttribute>();
            if (presenterAttribute != null)
            {
                var prefab = Resources.Load<GameObject>(presenterAttribute.PrefabPath);
                if (prefab != null)
                    Presenters.Add(presenterAttribute.Tag, new ElementPresenterInfo()
                    {
                        HtmlTag = presenterAttribute.Tag,
                        PresenterType = type,
                        Prefab = prefab,
                        IsContainer = presenterAttribute.IsContainer
                    });
                else
                    Debug.LogError("Invalid prefab path for IContentPresenter (" + type.Name + "): " + presenterAttribute.PrefabPath);
            }
            else Debug.LogError("IElementPresenter (" + type.Name + ") must have an ElementPresenterAttribute");
        }
    }

    public static bool HasTag(string Tag)
    {
        return Presenters.ContainsKey(Tag);
    }

    public static IElementPresenter Instantiate(Type PresenterType, IElement DataElement, IElementPresenter ParentElement)
    {
        var presenterInfo = Presenters.FirstOrDefault(i => i.Value.PresenterType == PresenterType).Value;
        return instantiate(presenterInfo, DataElement, ParentElement);
    }

    public static IElementPresenter Instantiate(IElement DataElement, IElementPresenter ParentElement)
    {
        var tagName = DataElement.TagName.ToUpper();
        if (!Presenters.ContainsKey(tagName)) return null;

        return instantiate(Presenters[tagName], DataElement, ParentElement);
    }

    private static IElementPresenter instantiate(ElementPresenterInfo presenterInfo, IElement dataElement, IElementPresenter parentElement)
    {
        Transform sceneContainer = null;
        if (parentElement != null && parentElement.SceneChildrenContainer != null) sceneContainer = parentElement.SceneChildrenContainer.transform;
        var presenter = GameObject.Instantiate(presenterInfo.Prefab, sceneContainer).GetComponent<IElementPresenter>();

        if (presenter == null)
        {
            Debug.LogError("Presenter for tag: " + presenterInfo.HtmlTag + ", not found on prefab");
            return null;
        }

        if (!string.IsNullOrEmpty(dataElement.Id))
            presenter.gameObject.name = dataElement.Id + " (" + dataElement.TagName + ")";
        else
            presenter.gameObject.name = dataElement.TagName;

        presenter.gameObject.name += " " + getUniqueIdForElement(dataElement);

        if (parentElement != null) parentElement.AddDOMChild(presenter);
        presenter.SetDOMParent(parentElement);
        presenter.ParseDataElement(dataElement);
        //presenter.Initialize();
        return presenter;
    }

    private static string getUniqueIdForElement(IElement dataElement)
    {
        //NOTE: it is important that this method return not only a unique id for the data element,
        //but also consistently return the same unique id for the same item across executions so that networking can be synced by game object name
        return dataElement.GetSelector().GetHashCode().ToString();
    }
}