using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[ElementPresenter("surface", "Presenters/Surface/Surface", true)]
public class SurfaceElementPresenter : ElementPresenterBase
{
    [SerializeField] private Transform ReferenceSurface;

    private LayoutSurface layoutSurface;
    public override ILayoutContainerInfo LayoutContainerInfo 
    { 
        get => layoutSurface; 
        protected set
        {
            Debug.LogWarning("Cannot set LayoutContainerInfo on SurfaceElementPresenter. It is handled internally");
        }
    }

    private void Awake()
    {
        layoutSurface = new LayoutSurface() { Bounds = new Bounds(Vector3.zero, new Vector3(ReferenceSurface.localScale.x, ReferenceSurface.localScale.y, ReferenceSurface.localScale.z)) };
        Debug.Log("Surface bounds set based on reference surface (Awake): ");
    }

    public override void ParseDataElement(IElement ElementData)
    {
        //TODO: get parameters from element data
    }

    public override async Task Initialize()
    {
    }
}
