using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class DocumentManager
{
    //public const string BROWSER_KEY = "775d447b-8333-42bc-882b-6926d91a14ae";

    //public struct PresenceInfoDto
    //{
    //    [JsonProperty("provider")]
    //    public string Provider { get; set; }

    //    [JsonProperty("key")]
    //    public string Key { get; set; }
    //}

    //public static async Task<PresenceInfoDto> FetchPresenceInfo(string BaseUrl)
    //{
    //    var url = new Uri(new Uri(BaseUrl), "/api/presence").AbsoluteUri;
    //    Debug.Log("Presence info url: " + url);

    //    using (var request = new UnityWebRequest(url, "GET"))
    //    {
    //        request.SetRequestHeader("Browser-Key", BROWSER_KEY);
    //        request.SetRequestHeader("User-Agent", "Thinkin/" + Application.version);
    //        if (UserInfo.CurrentUser != null) request.SetRequestHeader("auth", UserInfo.CurrentUser.AuthToken);
    //        request.downloadHandler = new DownloadHandlerBuffer();

    //        await request.SendWebRequest().GetTask();

    //        if (request.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("Presence Request error: " + request.error);
    //            return new PresenceInfoDto();
    //        }
    //        else
    //        {
    //            return JsonConvert.DeserializeObject<PresenceInfoDto>(request.downloadHandler.text);
    //        }
    //    }
    //}

    public static async Task<IDocument> FetchDocument(string Url)
    {
        var source = await getRequest(Url);
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(source).Address(Url));
        
        return document;
    }

    private static async Task<string> getRequest(string Url)
    {
        Debug.Log("Get request url: " + Url);
        using (var request = new UnityWebRequest(Url, "GET"))
        {
            request.SetRequestHeader("User-Agent", "Thinkin/" + Application.version);
            request.SetRequestHeader("Accept", "text/intervrse");
            //if (UserInfo.CurrentUser != null) request.SetRequestHeader("auth", UserInfo.CurrentUser.AuthToken); //TODO: reevaluate how to handle user authentication here
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().GetTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request error: " + request.error);
                return null;
            }
            else
            {
                return request.downloadHandler.text;
            }
        }
    }
}

public static class IElementPresenterExtensions
{
    public static IEnumerable<IElementPresenter> All(this IElementPresenter Presenter)
    {
        yield return Presenter;
        foreach(var child in Presenter.DOMChildren)
        {
            foreach(var item in child.All())
            {
                yield return item;
            }
        }
    }
}
