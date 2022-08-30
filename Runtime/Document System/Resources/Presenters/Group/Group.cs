using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("group", "Presenters/Group/Group", true)]
public class Group : ElementPresenterBase
{
    public override void ParseDataElement(IElement ElementData)
    {
        ApplyPlacement(ElementData, this.transform);
    }

    public override async Task Initialize()
    {
    }
}
