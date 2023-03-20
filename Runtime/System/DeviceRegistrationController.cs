using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;

public static class DeviceRegistrationController
{
    private static string filePath = Application.persistentDataPath + "/user.dat";

    public static async Task<UserInfo> GetUserInfo()
    {
        return await getUserInfoFromFile() ?? getUserInfoFromUserPrefs() ?? await LegacyDeviceRegistrationController.RegisterDeviceWithServer();
    }

    public static async Task ClearPersistedInfo()
    {
        await clearUserInfoInFile();
        clearUserInfoInUserPrefs();
        LegacyDeviceRegistrationController.ClearStoredInfo();
    }

    public static async Task PersistUserInfo(UserInfo User = null)
    {
        if (User == null) User = UserInfo.CurrentUser;

        var json = User.Serialize();
        await persistUserInfoToFile(json);
        persistUserInfoToUserPrefs(json);
    }

    private static async Task<UserInfo> getUserInfoFromFile()
    {
        if (!File.Exists(filePath)) return null;
        var json = await File.ReadAllTextAsync(filePath);
        if (string.IsNullOrEmpty(json)) return null;

        return UserInfo.Deserialize(json);
    }
    private static async Task persistUserInfoToFile(string json)
    {
        await File.WriteAllTextAsync(filePath, json);
    }
    private static async Task clearUserInfoInFile()
    {
        await File.WriteAllTextAsync(filePath, null);
    }


    private static UserInfo getUserInfoFromUserPrefs()
    {
        if (!PlayerPrefs.HasKey("userInfo")) return null;

        var json = PlayerPrefs.GetString("userInfo");
        return UserInfo.Deserialize(json);
    }
    private static void persistUserInfoToUserPrefs(string json)
    {
        PlayerPrefs.SetString("userInfo", json);
    }
    private static void clearUserInfoInUserPrefs()
    {
        PlayerPrefs.DeleteKey("userInfo");
    }
}


public static class LegacyDeviceRegistrationController
{
    //private static bool isListeningToWebsocket;

    private static string _uid;
    public static string UID
    {
        get
        {
            if (string.IsNullOrEmpty(_uid))
            {
                if (PlayerPrefs.HasKey("deviceUID"))
                {
                    _uid = PlayerPrefs.GetString("deviceUID");
                }
                else
                {
                    _uid = Guid.NewGuid().ToString();
                    PlayerPrefs.SetString("deviceUID", _uid);
                }
            }

            return _uid;
        }
    }

    //public static async void CheckDeviceRegistration() //Deprecated
    //{
    //    await RegisterDeviceWithServer();
    //    UserInfo.CurrentUser = await RegisterDeviceWithServer() ?? UserInfo.UnknownUser;

    //    if (UserInfo.CurrentUser != UserInfo.UnknownUser)
    //    {
    //        if (!isListeningToWebsocket)
    //        {
    //            isListeningToWebsocket = true;
    //            WebSocketListener.OnSocketConnected += WebSocketListener_OnSocketConnected;
    //        }
    //        //registerDeviceWithWebSocket();
    //        await DestinationPresenter.Instance.DisplayUrl(!string.IsNullOrEmpty(UserInfo.CurrentUser.CurrentRoomUrl) ? UserInfo.CurrentUser.CurrentRoomUrl : UserInfo.CurrentUser.HomeRoomUrl);
    //    }
    //    else await loadLoginRoom(); //await AppSceneManager.Instance.LoadLocalScene("Login");

    //}

    //private static async Task loadLoginRoom()
    //{
    //    await TransitionController.Instance.HideScene();
    //    await AppSceneManager.LoadLocalScene("Login");
    //    TransitionController.Instance.RevealScene();
    //}

    //private static void WebSocketListener_OnSocketConnected()
    //{
    //registerDeviceWithWebSocket();
    //}

    //private static void registerDeviceWithWebSocket()
    //{
    //    WebSocketListener.Socket.Emit("registerDevice", UserInfo.CurrentUser.AuthToken);
    //}


    public static async Task<UserInfo> RegisterDeviceWithServer()
    {
        Debug.Log("Registering device");

        var userDto = await WebAPI.RegisterDevice(UID, getMacAddress());
        if (userDto != null)
        {
            var user = new UserInfo()
            {
                Id = userDto.Id,
                AvatarUrl = userDto.AvatarUrl,
                DisplayName = userDto.DisplayName,
                //AuthToken = userDto.Token,
                HomeRoomUrl = userDto.HomeRoomUrl,
                CurrentRoomUrl = userDto.CurrentRoomUrl
            };
            return user;
            //UserInfo.CurrentUser = user;
        }
        else
        {
            return null;
            //UserInfo.CurrentUser = UserInfo.UnknownUser;
        }
    }

    public static void ClearStoredInfo()
    {
        //PlayerPrefs.DeleteKey("deviceUID"); //TODO: this was commented out just for testing

        //PlayerPrefs.DeleteAll();
        //UserInfo.CurrentUser = UserInfo.UnknownUser;
        //await loadLoginRoom(); 
    }


    private static string getMacAddress()
    {
        var macAdress = "";
        var nics = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var adapter in nics)
        {
            var address = adapter.GetPhysicalAddress();
            if (address.ToString() != "")
            {
                macAdress = address.ToString();
                return macAdress;
            }
        }
        return "UNKNOWN";
    }
}