using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("map", "Presenters/Map/Map", false)]
public class MapElementPresenter : ElementPresenterBase
{
    public override void ParseDataElement(IElement ElementData)
    {
        var portals = ElementData.QuerySelectorAll("portal");
        foreach(var portal in portals)
        {
            portal.SetAttribute("type", "PreviewOrb");
        }
    }

    public override async Task Initialize()
    {
    }

    //public override void ExecuteLayout()
    //{
    //    base.ExecuteLayout();
    //}
}