using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("scroll-area", "Presenters/ScrollArea/ScrollArea", true)]
public class ScrollAreaElementPresenter : ElementPresenterBase
{
    [SerializeField] private Transform Origin;

    public override void ParseDataElement(IElement ElementData)
    {
    }

    public override void ExecuteLayout()
    {
        base.ExecuteLayout();

        Bounds? bounds = null;
        foreach(var layoutChild in LayoutChildren)
        {
            var childBounds = layoutChild.GetBounds();
            if (childBounds.HasValue)
            {
                var localChildBounds = childBounds.Value;

                localChildBounds.center += layoutChild.transform.localPosition;
                localChildBounds.size.Scale(layoutChild.transform.localScale);

                if (!bounds.HasValue)
                {
                    bounds = localChildBounds;
                }
                else
                {
                    var v = bounds.Value;
                    v.Encapsulate(localChildBounds);
                    bounds = v;
                }
            }
        }
        this.BoundingBox = bounds.Value;

        centerViewOnContents();
    }

    private void centerViewOnContents()
    {
        if (LayoutContainerInfo == null) return;

        var layoutSurface = DOMParent.LayoutContainerInfo as LayoutSurface;
        if(layoutSurface == null)
        {
            Debug.LogError("ScrollAreaPresenter requires the LayoutContainerInfo to be a LayoutSurface");
            return;
        }
        var fitBox = layoutSurface.Bounds;

        var ratio = BoundingBox.Value.size.Divide(fitBox.size);
        var maxRatio = Mathf.Max(ratio.x, ratio.y);
        var minRatio = Mathf.Min(ratio.x, ratio.y);
        //var maxRatio = Mathf.Max(Mathf.Max(ratio.x, ratio.y), ratio.z);
        //var minRatio = Mathf.Min(Mathf.Min(ratio.x, ratio.y), ratio.z);

        transform.localPosition = fitBox.center;
        Origin.localScale = 1 / maxRatio * Vector3.one;
        Origin.localPosition = -(BoundingBox.Value.center * Origin.localScale.x) + (Vector3.forward * BoundingBox.Value.extents.z * Origin.localScale.x);
        //Origin.localPosition = -(BoundingBox.Value.center * (1 / maxRatio)) + (Vector3.forward * BoundingBox.Value.extents.z * (1 / maxRatio));
    }

    private void OnDrawGizmosSelected()
    {
        if (!BoundingBox.HasValue) return;

        Gizmos.DrawCube(BoundingBox.Value.center, BoundingBox.Value.size);
    }
}
