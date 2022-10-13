using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface ILayoutContainerInfo
{
}

public class LayoutSurface : ILayoutContainerInfo
{
    public Bounds Bounds;
}

public class LayoutVolume : ILayoutContainerInfo
{
    public Bounds Bounds;
}

[Obsolete]
public abstract class LayoutContainer : MonoBehaviour, ILayoutItem 
{
    public abstract Bounds? GetBounds();

    public abstract void ExecuteLayout();

    public abstract GameObject ContentContainer { get; }

    protected IEnumerable<ILayoutItem> GetChildLayoutItems()
    {
        yield return ContentContainer.GetComponent<ILayoutItem>();
        foreach(var item in ContentContainer.transform.GetChildren().SelectNotNull(i => i.GetComponent<ILayoutItem>()))
        {
            yield return item;
        }
    }

    public virtual void UpdateChildrenLayouts()
    {
        foreach(var child in GetChildLayoutItems())
        {
            child.ExecuteLayout();
        }
    }
}