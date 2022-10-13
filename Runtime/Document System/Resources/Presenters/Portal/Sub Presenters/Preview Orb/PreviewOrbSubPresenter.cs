using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class PreviewOrbSubPresenter : PortalSubPresenter
{
    [SerializeField] private TMPro.TextMeshPro Label;
    [SerializeField] private Transform Sphere;
    [SerializeField] private Renderer SphereRenderer;

    private static Material baseImageMaterial;

    private ILayoutItem layoutBounds;

    public override void Initialize(PortalElementPresenter ParentPortalPresenter)
    {
        base.Initialize(ParentPortalPresenter);
        Label.text = ParentPortalPresenter.Title;

        loadPreviewImage();
        layoutBounds = GetComponent<ILayoutItem>();
    }

    public override Bounds? GetBoundingBox()
    {
        return layoutBounds.GetBounds();
    }

    private async void loadPreviewImage()
    {
        var meta = await DocumentManager.FetchDocumentMeta(parentPortalPresenter.Href);
        var imageTag = meta.FirstOrDefault(i => i.GetAttribute("name") == "intervrse:image360");
        if(imageTag != null)
        {
            var imageUrl = imageTag.GetAttribute("content");
            SphereRenderer.sharedMaterial = await getImageMaterial(imageUrl);
        }
    }

    public override void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        base.OnHoverStart(Sender, RayInfo);
        Sphere.localScale = Vector3.one * 1.05f;
    }

    public override void OnHoverEnd(IUIPointer Sender)
    {
        base.OnHoverEnd(Sender);
        if(Sphere != null && Sphere.gameObject != null) Sphere.localScale = Vector3.one;
    }

    private static async Task<Material> getImageMaterial(string Url)
    {
        if (baseImageMaterial == null) baseImageMaterial = Resources.Load<Material>("Unlit Image");

        using (var request = UnityWebRequestTexture.GetTexture(Url))
        {
            await request.SendWebRequest().GetTask();
            var texture = DownloadHandlerTexture.GetContent(request);
            var material = Instantiate(baseImageMaterial);

            if(texture != null) material.mainTexture = texture;
            return material;
        }
    }
}
