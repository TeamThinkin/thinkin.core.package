using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanel : TabPanel
{
    [SerializeField] private Camera FrontCamera;
    [SerializeField] private Camera BackCamera;

    private Camera activeCamera;

    void Start()
    {
        FrontCamera.gameObject.SetActive(false);
        BackCamera.gameObject.SetActive(false);
        activateCamera(FrontCamera);
    }

    public void SwitchCameras()
    {
        activateCamera(activeCamera == FrontCamera ? BackCamera : FrontCamera);
    }

    private void activateCamera(Camera camera)
    {
        if (activeCamera != null) activeCamera.gameObject.SetActive(false);
        activeCamera = camera;
        activeCamera.gameObject.SetActive(true);
    }
}
