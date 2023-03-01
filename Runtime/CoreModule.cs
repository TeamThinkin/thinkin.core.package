using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public static class CoreModule
{
    public static async void Initialize()
    {
        var user = await DeviceRegistrationController.GetUserInfo() ?? UserInfo.UnknownUser;

        if (user != UserInfo.UnknownUser)
        {
            await DeviceRegistrationController.PersistUserInfo(user);
            UserInfo.CurrentUser = user;

            await AppSceneManager.LoadLocalScene("Home Room", true);
            //await DestinationPresenter.Instance.DisplayUrl(!string.IsNullOrEmpty(user.CurrentRoomUrl) ? user.CurrentRoomUrl : user.HomeRoomUrl); //TODO: this doesnt account for an empty Home Room Url

        }
        else
        {
            UserInfo.CurrentUser = user;
            await AppSceneManager.LoadLocalScene("Login", true);
        }
    }

    public static async Task LogoutUser()
    {
        await DeviceRegistrationController.ClearPersistedInfo();
        UserInfo.CurrentUser = UserInfo.UnknownUser;
    }
}
