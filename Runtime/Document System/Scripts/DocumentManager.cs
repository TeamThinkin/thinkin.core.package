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
    public static async Task<IDocument> FetchDocument(string Url)
    {
        var source = await getRequest(Url);
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(source).Address(Url));
        return document;
    }

    public static async Task<IElementPresenter> LoadDocumentIntoContainer(string Url, Transform ContentContainer, bool DisableEnvironemtPresenter = true)
    {
        var doc = await FetchDocument(Url);
        return await LoadDocumentIntoContainer(doc, ContentContainer, DisableEnvironemtPresenter);
    }

    public static async Task<IElementPresenter> LoadDocumentIntoContainer(IDocument Document, Transform ContentContainer, bool DisableEnvironemtPresenter = true)
    {
        var rootPresenter = ElementPresenterFactory.Instantiate(typeof(RootPresenter), Document.DocumentElement, null);
        rootPresenter.SetDOMParent(ContentContainer.GetComponentInParent<IElementPresenter>());
        rootPresenter.transform.SetParent(ContentContainer);
        rootPresenter.gameObject.name = "Root";
        rootPresenter.transform.Reset();

        traverseDOMforPresenters(Document.DocumentElement, rootPresenter);

        var presentersQuery = rootPresenter.All();
        if(DisableEnvironemtPresenter) presentersQuery = presentersQuery.Where(i => i is not EnvironmentElementPresenter);

        if (!DisableEnvironemtPresenter)
        {
            bool hasEnviornment = rootPresenter.All().Any(i => i is EnvironmentElementPresenter);
            if (!hasEnviornment) await AppSceneManager.LoadLocalScene("Empty Room");
        }

        await Task.WhenAll(presentersQuery.Select(i => i.Initialize()));

        rootPresenter.ExecuteLayout();

        return rootPresenter;
    }


    public static async Task<IHtmlCollection<IElement>> FetchDocumentMeta(string Url)
    {
        var doc = await FetchDocument(Url);
        return doc.QuerySelectorAll("meta");
    }

    private static void traverseDOMforPresenters(IElement dataElement, IElementPresenter parentPresenter, int depth = 0)
    {
        IElementPresenter currentPresenter = null;

        if (ElementPresenterFactory.HasTag(dataElement.TagName))
        {
            currentPresenter = ElementPresenterFactory.Instantiate(dataElement, parentPresenter);
        }

        foreach (var child in dataElement.Children)
        {
            traverseDOMforPresenters(child, currentPresenter ?? parentPresenter, depth + 1);
        }
    }

    private static async Task<string> getRequest(string Url)
    {
        Debug.Log("Get request url: " + Url);
        using (var request = new UnityWebRequest(Url, "GET"))
        {
            request.SetRequestHeader("User-Agent", "Thinkin/" + Application.version);
            request.SetRequestHeader("Accept", "text/intervrse");
            request.downloadHandler = new DownloadHandlerBuffer();

            await request.SendWebRequest().GetTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request error: " + request.error);
                return null;
            }
            else
            {
                Debug.Log("Get request complete for url: " + Url);
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
