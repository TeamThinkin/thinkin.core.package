using AngleSharp.Dom;
using AngleSharp.XPath;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

[ElementPresenter("picture", "Presenters/Picture/Picture", false)]
public class PictureElementPresenter : ElementPresenterBase
{
    [SerializeField] private Renderer ImageRenderer;
    [SerializeField] private Transform Visual;

    private static Dictionary<string, Task> pendingImageRequests = new Dictionary<string, Task>();
    private static Dictionary<string, Material> imageMaterials = new Dictionary<string, Material>();
    private static Material baseImageMaterial;

    private PlacementInfo placement;

    public bool HasVisual { get; private set; }

    private string src;

    public override void ParseDataElement(IElement ElementData)
    {
        src = ElementData.Attributes["src"].Value;
        placement = GetPlacementInfo(ElementData);
        ApplyPlacementPosition(placement, this.transform);
        ApplyPlacementRotation(placement, this.transform);
    }

    public override async Task Initialize()
    {
        bool IsSymbolic = false; //TODO: decide what to do with the symbolic thing

        var material = await GetImageMaterial(src);
        if (material != null && gameObject != null) //Bail out if the object has been destroyed while we were waiting to retrieve the image)
        {
            //var size = (Vector3)getWidthScaledSize(material.mainTexture.width, material.mainTexture.height, 1);
            var size = (Vector3)getHeightScaledSize(material.mainTexture.width, material.mainTexture.height, 1);
            size.z = 1f;
            ImageRenderer.sharedMaterial = material;

            if (placement.Scale.HasValue)
                Visual.localScale = placement.Scale.Value;
            else
                Visual.localScale = size;

            if (placement.Size.HasValue)
                this.transform.localScale = Vector3.one * placement.Size.Value;

            HasVisual = true;
        }

        //if (IsSymbolic)
        //{
        //    Destroy(GetComponent<GrabSyncMonitor>());
        //    Destroy(GetComponent<Autohand.DistanceGrabbable>());
        //    Destroy(GetComponent<Autohand.Grabbable>());
        //    Destroy(GetComponent<Rigidbody>());
        //}
    }

    public static async Task<Material> GetImageMaterial(string Url)
    {
        if (baseImageMaterial == null) baseImageMaterial = Resources.Load<Material>("Unlit Image");

        Material material = null;
        if (imageMaterials.ContainsKey(Url))
        {
            if (pendingImageRequests.ContainsKey(Url))
                await pendingImageRequests[Url];
            material = imageMaterials[Url];
        }
        else
        {
            material = Instantiate(baseImageMaterial);
            imageMaterials.Add(Url, material);

            using (var request = UnityWebRequestTexture.GetTexture(Url))
            {
                var task = request.SendWebRequest().GetTask();
                pendingImageRequests.Add(Url, task);
                await task;
                var texture = DownloadHandlerTexture.GetContent(request);
                material.mainTexture = texture;
                pendingImageRequests.Remove(Url);
            }
        }
        return material;
    }

    private Vector2 getWidthScaledSize(float originalWidth, float originalHeight, float targetWidth)
    {
        // w / h = W / H
        // h * W / w = H
        return new Vector2(targetWidth, originalHeight * targetWidth / originalWidth);
    }

    private Vector2 getHeightScaledSize(float originalWidth, float originalHeight, float targetHeight)
    {
        // w / h = W / H
        // h * W / w = H
        return new Vector2(originalWidth * targetHeight / originalHeight, targetHeight);
    }
}
