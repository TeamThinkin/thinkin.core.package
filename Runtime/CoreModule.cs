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


        //HomeRoomController.Instance.PromptUserForInviteCode(); //TODO: this needs to be refactored to only ask "new" users
        //HomeRoomController.Instance.WelcomeNewUser(); //TODO: need to implement logic for when to call this
    }

    public static async Task LogoutUser()
    {
        await DeviceRegistrationController.ClearPersistedInfo();
        UserInfo.CurrentUser = UserInfo.UnknownUser;
        AppSceneManager.LoadLocalScene("Home Room", true);
    }
}
