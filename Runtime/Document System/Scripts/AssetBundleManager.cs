using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class AssetBundleManager
{
    private static Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();

    public static async Task<AssetBundle> LoadAssetBundle(string Url)
    {
        return await LoadAssetBundle(new AssetUrl(Url));
    }

    public static async Task<AssetBundle> LoadAssetBundle(AssetUrl Address)
    { 
        
        if (loadedAssetBundles.ContainsKey(Address.CatalogUrl)) return loadedAssetBundles[Address.CatalogUrl];

        
        var request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(Address.CatalogUrl, 0);
        await request.SendWebRequest().GetTask();
        var bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);

        loadedAssetBundles.Add(Address.CatalogUrl, bundle);
        return bundle;
    }

    public static async Task UnloadAssetBundle(AssetBundle Bundle)
    {
        await Bundle.UnloadAsync(true).GetTask();

        if (loadedAssetBundles.ContainsValue(Bundle))
        {
            var entry = loadedAssetBundles.SingleOrDefault(i => i.Value == Bundle);
            loadedAssetBundles.Remove(entry.Key);
        }
    }

    public static async Task<GameObject> LoadPrefab(string Url)
    {
        return await LoadPrefab(new AssetUrl(Url));
    }

    public static async Task<GameObject> LoadPrefab(AssetUrl Address)
    {
        var bundle = await LoadAssetBundle(Address);
        var job = bundle.LoadAssetAsync<GameObject>(Address.AssetPath);
        await job.GetTask();
        return job.asset as GameObject;
    }
}
