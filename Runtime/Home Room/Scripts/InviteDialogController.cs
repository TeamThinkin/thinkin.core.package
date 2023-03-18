using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteDialogController : MonoBehaviour
{
    [SerializeField] private InviteCodeEntryController CodeEntryController;
    [SerializeField] private InviteDetailsController DetailsController;

    public void ShowDialog()
    {
        DetailsController.gameObject.SetActive(false);
        CodeEntryController.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        gameObject.SetActive(false);
    }

    public void ShowInviteDetails(InviteDto InviteDetailsDto)
    {
        CodeEntryController.gameObject.SetActive(false);
        DetailsController.ShowDetails(InviteDetailsDto);
    }
}
