using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeRoomController : MonoBehaviour
{
    [SerializeField] UserInfoPresenter UserPresenter;
    [SerializeField] InviteDialogController InviteDialog;

    public static HomeRoomController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        updateState();
        UserInfo.OnCurrentUserChanged += UserInfo_OnCurrentUserChanged;
    }

    private void OnDestroy()
    {
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;
    }

    private void UserInfo_OnCurrentUserChanged(UserInfo obj)
    {
        updateState();
    }

    private void updateState()
    {
        if (UserInfo.CurrentUser == null) return;

        UserPresenter.SetModel(UserInfo.CurrentUser);
    }

    public void OnChangeNameButtonPressed()
    {
        Debug.Log("Change name pressed");
    }

    public void PromptUserForInviteCode()
    {
        InviteDialog.ShowDialog();
    }
}
