using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("stack-panel", "Presenters/StackPanel/StackPanel", true)]
public class StackPanelElementPresenter : ElementPresenterBase
{
    [SerializeField] protected Vector3 StackDirection = Vector3.right;
    [SerializeField] protected float Padding;

    public override void ParseDataElement(IElement ElementData)
    {
        //TODO: parse values
    }

    public override void ExecuteLayout()
    {
        base.ExecuteLayout();

        Bounds? bounds = null;
        Vector3 runningPosition = Vector3.zero;

        foreach (var layoutChild in LayoutChildren)
        {
            var childBounds = layoutChild.GetBounds();
            if (childBounds.HasValue)
            {
                var localChildBounds = childBounds.Value;
                
                float size = Mathf.Abs(Vector3.Dot(StackDirection, childBounds.Value.size));

                layoutChild.transform.localPosition = runningPosition;
                localChildBounds.center += layoutChild.transform.localPosition;
                runningPosition += StackDirection * (size + Padding);

                if(!bounds.HasValue)
                {
                    bounds = localChildBounds;
                }
                else
                {
                    var newBounds = bounds.Value;
                    newBounds.Encapsulate(localChildBounds);
                    bounds = newBounds;
                }
            }
        }
        this.BoundingBox = bounds.Value;
    }
}
