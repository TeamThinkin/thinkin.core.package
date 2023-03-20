using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteDetailsPanel : TabPanel
{
    [SerializeField] private TMPro.TMP_Text InviterLabel;
    [SerializeField] private TMPro.TMP_Text InviteeLabel;
    [SerializeField] private TMPro.TMP_Text DestinationNameLabel;
    [SerializeField] private TMPro.TMP_Text DestinationUrlLabel;

    public bool HasDetails { get; private set; }

    private NewUserDialogController parentDialog;
    private InviteDto dto;

    private void Awake()
    {
        parentDialog = GetComponentInParent<NewUserDialogController>();
    }

    public void SetDetails(InviteDto Dto)
    {
        this.dto = Dto;

        HasDetails = true;
        InviterLabel.text = Dto.InviterDisplayName ?? "<Unknown>";
        InviteeLabel.text = Dto.UserDisplayName ?? "<Unknown>";

        if(!Dto.DestinationDisplayName.IsNullOrEmpty())
        {
            DestinationNameLabel.text = Dto.DestinationDisplayName;
            DestinationUrlLabel.text = Dto.DestinationUrl;
        }
        else
        {
            DestinationNameLabel.text = Dto.DestinationUrl;
            DestinationUrlLabel.text = "";
        }
    }

    public void AcceptInvite()
    {
        var newUser = new UserInfo();
        newUser.AvatarUrl = dto.AvatarUrl ?? UserInfo.CurrentUser.AvatarUrl;
        newUser.DisplayName = dto.UserDisplayName ?? UserInfo.CurrentUser.DisplayName;
        newUser.HomeRoomUrl = dto.DestinationUrl;
        UserInfo.CurrentUser = newUser;
        DeviceRegistrationController.PersistUserInfo();
        parentDialog.NextPanel();
    }
}
