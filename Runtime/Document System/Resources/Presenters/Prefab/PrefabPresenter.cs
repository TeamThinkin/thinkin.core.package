using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("prefab", "Presenters/Prefab/Prefab", false)]
public class PrefabPresenter : ElementPresenterBase
{
    private string src;
    private PlacementInfo placement;

    public override void ParseDataElement(IElement ElementData)
    {
        src = ElementData.Attributes["src"]?.Value;
        placement = GetPlacementInfo(ElementData);
    }
    public override async Task Initialize()
    {
        ApplyPlacement(placement, this.transform);

        if (!string.IsNullOrEmpty(src))
        {
            var address = new AssetUrl(src);
            var bundle = await AssetBundleManager.LoadAssetBundle(address);
            var prefab = bundle.LoadAsset<GameObject>(address.AssetPath);
            var instance = Instantiate(prefab, this.transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
            Debug.Log("Prefab Presenter's item is instantiated");
        }
        else Debug.Log("PrefabPresenter src is null");
    }
}
