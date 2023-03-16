using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserInfo
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("avatarUrl")]
    public string AvatarUrl { get; set; }

    //public string AuthToken { get; set; }

    [JsonProperty("homeRoomUrl")]
    public string HomeRoomUrl { get; set; }

    [JsonProperty("currentRoomUrl")]
    public string CurrentRoomUrl { get; set; }

    //public string RootCollectionUrl => "user-" + Id;

    public static event Action<UserInfo> OnCurrentUserChanged;
    private static UserInfo _unknownUser = new UserInfo()
    {
        DisplayName = CodewordGenerator.GetSimpleName()
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

    public string Serialize()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static UserInfo Deserialize(string Json)
    {
        if (string.IsNullOrEmpty(Json)) return null;

        return JsonConvert.DeserializeObject<UserInfo>(Json);
    }
}