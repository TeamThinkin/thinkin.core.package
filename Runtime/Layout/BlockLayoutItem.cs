using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLayoutItem : MonoBehaviour, ILayoutItem
{
    [SerializeField] private Transform BlockReference;

    private Bounds bounds = new Bounds();

    public Bounds? GetBounds()
    {
        bounds.center = BlockReference.localPosition;
        bounds.size = BlockReference.localScale;
        return bounds;
    }

    public void ExecuteLayout() {}
}
