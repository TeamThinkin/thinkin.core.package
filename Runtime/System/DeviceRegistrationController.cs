using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using UnityEngine;

public static class DeviceRegistrationController
{
    private static bool isListeningToWebsocket;

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

    public static async void CheckDeviceRegistration()
    {
        await RegisterDevice();

        if (UserInfo.CurrentUser != UserInfo.UnknownUser)
        {
            if (!isListeningToWebsocket)
            {
                isListeningToWebsocket = true;
                WebSocketListener.OnSocketConnected += WebSocketListener_OnSocketConnected;
            }
            registerDeviceWithWebSocket();
            await DestinationPresenter.Instance.DisplayUrl(!string.IsNullOrEmpty(UserInfo.CurrentUser.CurrentRoomUrl) ? UserInfo.CurrentUser.CurrentRoomUrl : UserInfo.CurrentUser.HomeRoomUrl);
        }
        else await loadLoginRoom(); //await AppSceneManager.Instance.LoadLocalScene("Login");

    }

    private static async Task loadLoginRoom()
    {
        await TransitionController.Instance.HideScene();
        await AppSceneManager.LoadLocalScene("Login");
        TransitionController.Instance.RevealScene();
    }

    private static void WebSocketListener_OnSocketConnected()
    {
        registerDeviceWithWebSocket();
    }

    private static void registerDeviceWithWebSocket()
    {
        WebSocketListener.Socket.Emit("registerDevice", UserInfo.CurrentUser.AuthToken);
    }

    public static async Task RegisterDevice()
    {
        var userDto = await WebAPI.RegisterDevice(UID, getMacAddress());
        if (userDto != null)
        {
            var user = new UserInfo()
            {
                Id = userDto.Id,
                AvatarUrl = userDto.AvatarUrl,
                DisplayName = userDto.DisplayName,
                AuthToken = userDto.Token,
                HomeRoomUrl = userDto.HomeRoomUrl,
                CurrentRoomUrl = userDto.CurrentRoomUrl
            };
            UserInfo.CurrentUser = user;
        }
        else
        {
            UserInfo.CurrentUser = UserInfo.UnknownUser;
        }
    }

    public static async void Logout()
    {
        PlayerPrefs.DeleteKey("deviceUID");
        //PlayerPrefs.DeleteAll();
        UserInfo.CurrentUser = UserInfo.UnknownUser;
        await loadLoginRoom(); 
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
                Debug.Log("Mac Address retrieved: " + macAdress);
                return macAdress;
            }
        }
        return "UNKNOWN";
    }
}
