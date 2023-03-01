using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoPresenter : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text Label;

    public void SetModel(UserInfo User)
    {
        if (UserInfo.CurrentUser != null)
            Label.text = "Welcome " + User.DisplayName;
        else
            Label.text = "Not logged in";
    }
}
