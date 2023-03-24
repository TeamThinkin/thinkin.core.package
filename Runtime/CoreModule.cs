using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class CoreModule
{
    public static async void Initialize()
    {
        var user = await DeviceRegistrationController.GetUserInfo() ?? UserInfo.UnknownUser;

        if (user != UserInfo.UnknownUser) await DeviceRegistrationController.PersistUserInfo(user);
        
        UserInfo.CurrentUser = user;

        await AppSceneManager.LoadLocalScene("Home Room", true);
    }

    public static async Task LogoutUser()
    {
        await DeviceRegistrationController.ClearPersistedInfo();
        UserInfo.CurrentUser = UserInfo.UnknownUser;
        await AppSceneManager.LoadLocalScene("Home Room", true);
    }
}
