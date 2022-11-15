using System.Collections;
using UnityEngine;

public static class CoreModule
{
    public static void Initialize()
    {
        DeviceRegistrationController.CheckDeviceRegistration();
    }
}
