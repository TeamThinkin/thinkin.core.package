using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoPresenter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text DisplayNameLabel;
    [SerializeField] private Textbox DisplayNameTextbox;
    [SerializeField] private GameObject DisplayModeContainer;
    [SerializeField] private GameObject EditModeContainer;

    public void SetModel(UserInfo User)
    {
        DisplayModeContainer.SetActive(true);
        EditModeContainer.SetActive(false);

        if (UserInfo.CurrentUser != null)
            DisplayNameLabel.text = User.DisplayName;
        else
            DisplayNameLabel.text = "Not logged in";
    }

    public void EnterEditMode()
    {
        DisplayNameTextbox.Text = DisplayNameLabel.text;
        DisplayModeContainer.SetActive(false);
        EditModeContainer.SetActive(true);
        FocusManager.SetFocus(DisplayNameTextbox);
    }

    public void SaveChanges()
    {
        DisplayNameLabel.text = DisplayNameTextbox.Text;
        UserInfo.CurrentUser.DisplayName = DisplayNameTextbox.Text;
        _ = DeviceRegistrationController.PersistUserInfo();
        DisplayModeContainer.SetActive(true);
        EditModeContainer.SetActive(false);
    }

    public void CancelChanges()
    {
        DisplayModeContainer.SetActive(true);
        EditModeContainer.SetActive(false);
    }
}
