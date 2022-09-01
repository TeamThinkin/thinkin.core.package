using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public string Id;
    public string DisplayName;
    public string AvatarUrl;
    public string AuthToken;
    public string HomeRoomUrl;
    public string CurrentRoomUrl;

    public string RootCollectionUrl => "user-" + Id;

    public static event Action<UserInfo> OnCurrentUserChanged;
    private static UserInfo _unknownUser = new UserInfo()
    {
        DisplayName = "Unkown User"
    };
    public static UserInfo UnknownUser => _unknownUser;


    private static UserInfo _currentUser;
    public static UserInfo CurrentUser
    {
        get { return _currentUser; }
        set
        {
            if (_currentUser == value) return;
            _currentUser = value;
            OnCurrentUserChanged?.Invoke(_currentUser);
        }
    }

    public static bool IsLoggedIn => CurrentUser != null && CurrentUser != UnknownUser;
}