using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSceneManager
{
    public static event Action OnEnvironmentLoaded;
    public static event Action OnEnvironmentUnloaded;

    private static AssetBundle currentAssetBundle;
    private static bool currentSceneIsRemote;
    private static string currentScene;
    private static string currentSceneUrl;
    private static string currentCatalogUrl;
    private static bool isLoading;

    public static async Task LoadLocalScene(string SceneName, bool useTransition = false)
    {
        if (isLoading) return;
        if (currentScene == SceneName) return; //NOTE: when this line was inserted it meant that OnEnvironmentLoaded would no longer be invoked

        isLoading = true;

        if (useTransition) await TransitionController.Instance.HideScene();

        OnEnvironmentUnloaded?.Invoke();

        await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single).GetTask();
        disableExtraCameras();

        currentScene = SceneName;
        currentSceneUrl = null;
        currentSceneIsRemote = false;
        isLoading = false;

        OnEnvironmentLoaded?.Invoke();

        if (useTransition) await TransitionController.Instance.RevealScene();
    }

    public static async Task LoadRemoteScene(string SceneUrl)
    {
        if (isLoading) return;
        if (currentScene != SceneUrl)
        {

            isLoading = true;
            OnEnvironmentUnloaded?.Invoke();
            currentScene = SceneUrl;
            currentSceneIsRemote = true;
            await loadRemoteScene(SceneUrl);
            isLoading = false;
        }
        OnEnvironmentLoaded?.Invoke();
    }


    private static async Task loadRemoteScene(string SceneUrl)
    {
        if (currentSceneUrl == SceneUrl) return;

        var address = new AssetUrl(SceneUrl);

        if (currentCatalogUrl != address.CatalogUrl)
        {
            currentAssetBundle?.Unload(true);
            var request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(address.CatalogUrl, 0);
            await request.SendWebRequest().GetTask();
            currentAssetBundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);
        }

        string scenePath = address.AssetPath;
        if (string.IsNullOrEmpty(scenePath)) scenePath = currentAssetBundle.GetAllScenePaths()[0];

        await SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Single).GetTask();
        disableExtraCameras();

        currentScene = scenePath;
        currentSceneUrl = SceneUrl;
        currentSceneIsRemote = true;
    }

    private static void disableExtraCameras()
    {
        //In the even the a loaded scene already has a camera in it, we want to disable it so it doesnt fuck up the user's main camera
        var cameras = GameObject.FindObjectsOfType<Camera>();
        var mainCamera = AppControllerBase.Instance.MainCamera;
        foreach (var camera in cameras)
        {
            if (camera != mainCamera && camera.targetTexture == null)
            {
                Debug.Log("Extra camera found in scene. Disabling it. " + camera.gameObject.name, camera.gameObject);
                camera.gameObject.SetActive(false);
            }
        }
    }
}
