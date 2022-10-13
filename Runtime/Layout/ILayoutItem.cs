using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILayoutItem
{
    Bounds? GetBounds();
    void ExecuteLayout();

    GameObject gameObject { get; }
    Transform transform { get; }
}