using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeRoomController : MonoBehaviour
{
    [SerializeField] UserInfoPresenter UserPresenter;
    
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
}
