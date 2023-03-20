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

    public void SetDetails(InviteDto Dto)
    {
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
}
