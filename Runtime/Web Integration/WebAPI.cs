using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class WebAPI
{
    public const string ApiVersion = "v1";

    public static string HomeServerApiBaseUrl
    {
        get { return "https://" + Config.HomeServerAuthorityAddress + "/" + ApiVersion + "/"; }
    }

    public static string GetServerApiBaseUrlFromUrl(string Url)
    {
        Uri uri = new Uri(Url);
        return uri.Scheme + "://" + uri.Authority + "/" + ApiVersion + "/";
    }

    public static string GetCollectionUrl(string ServerApiBaseUrl, string CollectionKey)
    {
        return ServerApiBaseUrl + "auth/collection/" + CollectionKey;
    }

    public static async Task<RegisterDeviceResultDto> RegisterDevice(string Uid, string MacAddress)
    {
        return await postRequest<RegisterDeviceResultDto>(HomeServerApiBaseUrl + "device/register", new RegisterDeviceRequestDto() { Uid = Uid, Mac = MacAddress });
    }

    public static async Task<RegistryEntryDto[]> Maps()
    {
        return await getRequest<RegistryEntryDto[]>(HomeServerApiBaseUrl + "auth/map");
    }

    public static async Task<RegistryEntryDto[]> Destinations()
    {
        return await getRequest<RegistryEntryDto[]>(HomeServerApiBaseUrl + "auth/destination");
    }

    public static async Task<CollectionContentItemDto[]> GetCollectionContents(string CollectionUrl)
    {
        var result = await getRequest<CollectionContentItemDto[]>(NormalizeCollectionUrl(CollectionUrl), new ContentItemDtoConverter());
        result = result.WhereNotNull().ToArray();
        return result;
    }

    public static async Task<string> AddMapDestination(string MapUrl, string MapKey, AddMapDestinationDto Dto)
    {
        string apiBase = GetServerApiBaseUrlFromUrl(MapUrl);
        return await postRequest<string>(apiBase + "auth/map/" + MapKey + "/destination", Dto);
    }

    public static string NormalizeCollectionUrl(string Url)
    {
        //Url might be a fully qualified url like:
        // https://thinkin-api.glitch.me/v1/auth/collection/the-collection-id
        //or it might be a relative one like:
        // the-collection-id
        if (Url.ToLower().StartsWith("http")) return Url;

        return HomeServerApiBaseUrl + "auth/collection/" + Url;
    }

    public static async Task UpdateCollectionItem(string CollectionUrl, CollectionContentItemDto Dto)
    {
        string url = NormalizeCollectionUrl(CollectionUrl) + "/" + Dto.Id;
        await requestWithBody("PUT", url, Dto);
    }

    public static async Task<TResult> postRequest<TResult>(string Url, object body)
    {
        Debug.Log("POST request: " + Url);
        using (var request = new UnityWebRequest(Url, "POST"))
        {
            var json = body.ToJSON();
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            request.SetRequestHeader("Content-Type", "application/json");
            if(!string.IsNullOrEmpty(UserInfo.CurrentUser?.AuthToken)) request.SetRequestHeader("auth", UserInfo.CurrentUser.AuthToken);
            request.uploadHandler = new UploadHandlerRaw(bytes);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().GetTask();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request error: " + request.error);
                return default(TResult);
            }
            else
            {
                return JsonConvert.DeserializeObject<TResult>(request.downloadHandler.text);
            }
        }
    }

    public static async Task<TResult> putRequest<TResult>(string Url, object body)
    {
        return await requestWithBody<TResult>("PUT", Url, body);
    }

    public static async Task<TResult> requestWithBody<TResult>(string Verb, string Url, object body)
    {
        var result = await requestWithBody(Verb, Url, body);
        if (result != null)
            return JsonConvert.DeserializeObject<TResult>(result);
        else
            return default(TResult);
    }

    public static async Task<string> requestWithBody(string Verb, string Url, object body)
    {
        Debug.Log(Verb + " request: " + Url);
        using (var request = new UnityWebRequest(Url, Verb))
        {
            var json = body.ToJSON();
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            request.SetRequestHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(UserInfo.CurrentUser?.AuthToken)) request.SetRequestHeader("auth", UserInfo.CurrentUser.AuthToken);
            request.uploadHandler = new UploadHandlerRaw(bytes);
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

    private static async Task<T> getRequest<T>(string Url, JsonConverter Converter = null)
    {
        Debug.Log("Get request: " + Url);
        using (var request = new UnityWebRequest(Url, "GET"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            if (UserInfo.CurrentUser != null) request.SetRequestHeader("auth", UserInfo.CurrentUser.AuthToken);
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().GetTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request error: " + request.error);
                return default(T);
            }
            else
            {
                if(Converter != null)
                    return JsonConvert.DeserializeObject<T>(request.downloadHandler.text, Converter);
                else
                    return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            }
        }
    }
}
