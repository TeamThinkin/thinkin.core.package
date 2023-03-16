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

    public static async Task LoadLocalScene(string SceneName)
    {
        if (isLoading) return;
        if (currentScene != SceneName)
        {
            isLoading = true;
            //for (int i=0;i<SceneManager.sceneCount;i++)
            //{
            //    if (SceneManager.GetSceneAt(i).name == SceneName) return;
            //}

            OnEnvironmentUnloaded?.Invoke();

            await SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single).GetTask();
            disableExtraCameras();

            currentScene = SceneName;
            currentSceneUrl = null;
            currentSceneIsRemote = false;
            isLoading = false;
            DestinationPresenter.Instance.ResetCurrentDestination(); //TODO: reevaulate the logic here
        }
        OnEnvironmentLoaded?.Invoke();

        
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

        if(currentCatalogUrl != address.CatalogUrl)
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
        var cameras = GameObject.FindObjectsOfType<Camera>();
        var mainCamera = AppControllerBase.Instance.MainCamera;
        foreach (var camera in cameras)
        {
            if (camera != mainCamera) camera.gameObject.SetActive(false);
        }
    }
}
