using AngleSharp.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("environment", "Presenters/Environment/Environment", false)]
public class EnvironmentElementPresenter : ElementPresenterBase
{
    string typeName;
    string src;

    public override void ParseDataElement(IElement ElementData)
    {
        typeName = ElementData.Attributes["type"].Value;
        src = ElementData.Attributes["src"].Value;
    }

    public override async Task Initialize()
    {
        switch(typeName.ToLower())
        {
            case "unity.assetbundle":
                await AppSceneManager.LoadRemoteScene(src);
                break;
            case "local":
                await AppSceneManager.LoadLocalScene(src);
                break;
            default:
                Debug.LogError("Unrecognized Environment type: " + typeName);
                break;
        }       
    }
}
