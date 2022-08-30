using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("", "Presenters/Root/Root", true)]
public class RootPresenter : ElementPresenterBase
{
    public override void ParseDataElement(IElement ElementData)
    {
    }

    public override async Task Initialize()
    {
    }
}
