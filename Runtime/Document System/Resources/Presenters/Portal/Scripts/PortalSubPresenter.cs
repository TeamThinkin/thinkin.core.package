using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class PortalSubPresenter : MonoBehaviour, IHandlePointerEvent
{
    //protected PortalElementPresenter parentPortalPresenter;
    protected string Title;
    protected string Href;
    protected bool isLocalDestination;
    protected Vector3? localDestinationPosition;
    protected Vector3? localDestinationEuler;

    public virtual void Initialize(string Title, string Href)
    {
        this.Title = Title;
        this.Href = Href;
        parseHref();
    }

    public virtual Bounds? GetBoundingBox()
    {
        return null;
    }

    public virtual void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        _ = ActivatePortal();
    }

    public async Task ActivatePortal()
    {
        if (isLocalDestination)
        {
            Debug.Log("Local destination");
            if (localDestinationEuler.HasValue) AppControllerBase.Instance.SetPlayerRotation(Quaternion.Euler(localDestinationEuler.Value));
            if (localDestinationPosition.HasValue) AppControllerBase.Instance.SetPlayerPosition(localDestinationPosition.Value);
        }
        else
        {
            await DestinationPresenter.Instance.DisplayUrl(Href);
        }
    }

    private void parseHref()
    {
        var hashIndex = Href.IndexOf("#");
        isLocalDestination = hashIndex > -1;
        if (isLocalDestination)
        {

            var hash = Href.Substring(hashIndex + 1);
            hash = Thinkin.Web.HttpUtility.UrlDecode(hash);
            // Assumed format: {0.1,0.2,0.3},{0.2,0.4,0.6}

            int startBrace = 0;
            int endBrace = 0;
            string chunk;
            for (int i = 0; i < 2; i++)
            {
                startBrace = hash.IndexOf('{', endBrace);

                if (startBrace > -1)
                {
                    endBrace = hash.IndexOf('}', startBrace);

                    if (endBrace > -1)
                    {
                        chunk = hash.Substring(startBrace, endBrace - startBrace + 1);
                        if (i == 0)
                            localDestinationPosition = chunk.ToVector3(); // parseVector3String(chunk);
                        else if (i == 1)
                            localDestinationEuler = chunk.ToVector3(); // parseVector3String(chunk);
                    }
                    else break;
                }
                else break;
            }

            if (localDestinationPosition.HasValue) Debug.Log("Destination position: " + localDestinationPosition.Value);
            if (localDestinationEuler.HasValue) Debug.Log("Destination euler: " + localDestinationEuler.Value);
        }
    }

    #region -- Unused Pointer events --
    public virtual void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo) { }

    public virtual void OnHoverEnd(IUIPointer Sender) { }

    public virtual void OnGripStart(IUIPointer Sender, RaycastHit RayInfo) { }

    public virtual void OnGripEnd(IUIPointer Sender) { }

    public virtual void OnTriggerEnd(IUIPointer Sender) { }
    #endregion
}
