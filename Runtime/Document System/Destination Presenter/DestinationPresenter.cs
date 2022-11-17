using AngleSharp.Dom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class DestinationPresenter : MonoBehaviour
{
    public static event Action<string> UrlChanged;
    public static event Action OnDestinationLoaded;
    public static event Action OnDestinationUnloaded;

    public static DestinationPresenter Instance { get; private set; }
    public static int? CurrentDestinationId { get; private set; }

    [SerializeField] private Transform _contentContainer;
    public Transform ContentItemContainer => _contentContainer;

    [SerializeField] private TransitionController _transitionController;

    public IElementPresenter RootPresenter { get; private set; }

    public string CurrentUrl { get; private set; }
    //public DocumentManager.PresenceInfoDto CurrentPresenceInfo { get; private set; }

    public bool IsLoading { get; private set; }

    

    private void Awake()
    {
        Instance = this;
    }

    public async Task DisplayUrl(string Url)
    {
        //NOTE: Assumes Urls are fully formed and not relative links

        var newRoomId = Url.GetHashCode();
        if (newRoomId == CurrentDestinationId) return;

        IsLoading = true;
        CurrentUrl = Url;
        UrlChanged?.Invoke(Url);

        CurrentDestinationId = newRoomId;

        var documentTask = DocumentManager.FetchDocument(Url);

        await _transitionController.HideScene();
        
        stashPlayer();

        OnDestinationUnloaded?.Invoke();
        _contentContainer.ClearChildren();

        var document = await documentTask;
        RootPresenter = await DocumentManager.LoadDocumentIntoContainer(document, _contentContainer, false);

        OnDestinationLoaded?.Invoke();
        _transitionController.RevealScene();

        releaseStashedPlayer();
        IsLoading = false;
    }

    private void stashPlayer()
    {
        var playerBody = AppControllerBase.Instance.PlayerBody;
        if(playerBody == null) return;

        playerBody.isKinematic = true;
        playerBody.velocity = Vector3.zero;
        playerBody.angularVelocity = Vector3.zero;
        AppControllerBase.Instance.SetPlayerPosition(Vector3.one * -1000);
    }

    private void releaseStashedPlayer()
    {
        var playerBody = AppControllerBase.Instance.PlayerBody;
        if (playerBody == null) return;

        playerBody.isKinematic = false;
    }

    //private async Task loadDocument(IDocument Document)
    //{
    //    RootPresenter = ElementPresenterFactory.Instantiate(typeof(RootPresenter), Document.DocumentElement, null);
    //    RootPresenter.transform.SetParent(_contentContainer);
    //    RootPresenter.gameObject.name = "Root";

    //    traverseDOMforPresenters(Document.DocumentElement, RootPresenter);
        
    //    bool hasEnviornment = RootPresenter.All().Any(i => i is EnvironmentElementPresenter);
    //    if(!hasEnviornment) await AppSceneManager.LoadLocalScene("Empty Room");
        
    //    await Task.WhenAll(RootPresenter.All().Select(i => i.Initialize()));
    //}

    //private static void traverseDOMforPresenters(IElement dataElement, IElementPresenter parentPresenter, int depth = 0)
    //{
    //    IElementPresenter currentPresenter = null;

    //    if (ElementPresenterFactory.HasTag(dataElement.TagName))
    //    {
    //        currentPresenter = ElementPresenterFactory.Instantiate(dataElement, parentPresenter);
    //    }

    //    foreach (var child in dataElement.Children)
    //    {
    //        traverseDOMforPresenters(child, currentPresenter ?? parentPresenter, depth + 1);
    //    }
    //}
}
