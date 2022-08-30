using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DocumentInspector : MonoBehaviour
{
    public async Task DisplayUrl(string Url)
    {
        Debug.Log("Inspector display url: " + Url);
        var documentTask = DocumentManager.FetchDocument(Url);

        transform.ClearChildren();

        var document = await documentTask;
        await loadDocument(document);

        Debug.Log("Inspector display complete");
    }

    private async Task loadDocument(IDocument Document)
    {
        //var rootPresenter = ElementPresenterFactory.Instantiate(typeof(RootPresenter), Document.DocumentElement, null);
        //rootPresenter.transform.SetParent(transform);
        //rootPresenter.gameObject.name = "Root";

        traverseDOMforPresenters(Document.DocumentElement);

        //await Task.WhenAll(rootPresenter.All().Select(i => i.Initialize()));
    }

    private static void traverseDOMforPresenters(IElement dataElement, int depth = 0)
    {
        if (ElementPresenterFactory.HasTag(dataElement.TagName))
        {
            var info = ElementPresenterFactory.Presenters[dataElement.TagName];
            Debug.Log(new string('-', depth) + dataElement.TagName + "(IsContainer: " + info.IsContainer + ")");
        }

        foreach (var child in dataElement.Children)
        {
            traverseDOMforPresenters(child, depth + 1);
        }
    }
}
