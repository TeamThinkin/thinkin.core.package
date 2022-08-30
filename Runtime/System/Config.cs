using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config
{
#if UNITY_EDITOR
    /*public const string PlatformKey = "StandaloneWindows";*/
    public const string PlatformKey = "Windows64";
#elif UNITY_STANDALONE || UNITY_EDITOR
    public const string PlatformKey = "StandaloneWindows64";
#elif UNITY_ANDROID
    public const string PlatformKey = "Android";
#endif

    public const string DefaultHomeServerAuthortyAddress = "thinkin-api.glitch.me";

    private static string _homeServerAuthorityAddress;
    /// <summary>
    /// Example: thinkin-api.glitch.me
    /// </summary>
    public static string HomeServerAuthorityAddress
    {
        get 
        { 
            if(_homeServerAuthorityAddress == null)
            {
                if(PlayerPrefs.HasKey("HomeServerAuthorityAddress"))
                    _homeServerAuthorityAddress = PlayerPrefs.GetString("HomeServerAuthorityAddress");
                else 
                    _homeServerAuthorityAddress = DefaultHomeServerAuthortyAddress;
                
            }
            return _homeServerAuthorityAddress; 
        }
        set 
        {
            PlayerPrefs.SetString("HomeServerAuthorityAddress", _homeServerAuthorityAddress);
        }
    }
}
