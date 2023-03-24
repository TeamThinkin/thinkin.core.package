using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickLinksController : MonoBehaviour
{
    [SerializeField] private PreviewOrbSubPresenter HomeRoomPreviewOrb;
    [SerializeField] private PreviewOrbSubPresenter InviteDestinationPreviewOrb;
    [SerializeField] private PreviewOrbSubPresenter LastLocationPreviewOrb;

    private string inviteDestinationUrl;
    private string inviteDestinationTitle;

    public void UpdateInfo()
    {
        //updateLink(UserInfo.CurrentUser.HomeRoomInfo, HomeRoomPreviewOrb);
        updateLink(UserInfo.CurrentUser.CurrentRoomInfo, LastLocationPreviewOrb);
        updateLink(new DocumentMetaInformation() { Title = inviteDestinationTitle, DocumentUrl = inviteDestinationUrl }, InviteDestinationPreviewOrb);
    }

    public void SetInviteDestination(string Title, string Url)
    {
        inviteDestinationTitle = Title;
        inviteDestinationUrl = Url;
        UpdateInfo();
    }

    private void updateLink(DocumentMetaInformation docMeta, PreviewOrbSubPresenter previewOrb)
    {
        if (docMeta.DocumentUrl.IsNullOrEmpty())
        {
            previewOrb.transform.parent.gameObject.SetActive(false);
            return;
        }

        previewOrb.transform.parent.gameObject.SetActive(true);
        previewOrb.Initialize(docMeta.Title, docMeta.DocumentUrl, docMeta.IconUrl);
    }
}
