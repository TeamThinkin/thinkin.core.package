using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPanel : TabPanel
{
    [SerializeField] private Textbox UrlTextbox;
    [SerializeField] private DropDownBox MapsDropDownBox;
    [SerializeField] private GameObject MapContents;
    [SerializeField] private GameObject LoadingIndicator;
    
    private string selectedMapUrl;

    private void Awake()
    {
        MapsDropDownBox.SelectedItemChanged += MapsDropDownBox_SelectedItemChanged;
        UrlTextbox.OnEnterPressed += UrlTextbox_OnEnterPressed;
        UrlTextbox.OnFocusLost += UrlTextbox_OnFocusLost;
    }

    protected override void OnShow()
    {
        base.OnShow();
        UrlTextbox.Text = DestinationPresenter.Instance.CurrentUrl;

        loadMapList();
        UserInfo.OnCurrentUserChanged += UserInfo_OnCurrentUserChanged;
        DestinationPresenter.UrlChanged += DestinationPresenter_UrlChanged;
        
    }

    protected override void OnHide()
    {
        Debug.Log("Map panel hide");
        base.OnHide();
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;
        DestinationPresenter.UrlChanged -= DestinationPresenter_UrlChanged;
    }

    private void OnDestroy()
    {
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;
        MapsDropDownBox.SelectedItemChanged -= MapsDropDownBox_SelectedItemChanged;
        UrlTextbox.OnEnterPressed -= UrlTextbox_OnEnterPressed;
        UrlTextbox.OnFocusLost -= UrlTextbox_OnFocusLost;
    }

    public async void NavigateHome()
    {
        //await DestinationPresenter.Instance.DisplayUrl(UserInfo.CurrentUser.HomeRoomUrl);
        await AppSceneManager.LoadLocalScene("Home Room", true);
    }

    public async void NavigateBack()
    {
        await DestinationPresenter.Instance.NavigateBack();
    }

    public async void NavigateForward()
    {
        await DestinationPresenter.Instance.NavigateForward();
    }

    public async void RefreshPage()
    {
        await DestinationPresenter.Instance.Refresh();
    }

    private void UrlTextbox_OnFocusLost(Textbox obj)
    {
        Debug.Log("url focus lost");
    }

    private async void UrlTextbox_OnEnterPressed(Textbox obj)
    {
        Debug.Log("url enter pressed: " + obj.Text);
        FocusManager.ClearFocus();
        await DestinationPresenter.Instance.DisplayUrl(UrlTextbox.Text);
    }


    private void UserInfo_OnCurrentUserChanged(UserInfo obj)
    {
        loadMapList();
    }


    private void DestinationPresenter_UrlChanged(string newUrl)
    {
        UrlTextbox.Text = newUrl;
        loadMapList();
    }

    private void MapsDropDownBox_SelectedItemChanged(ListItemDto selectedItem)
    {
        string url = selectedItem.Value as string;
        if (selectedMapUrl == url) return;

        selectedMapUrl = url;
        populateMap(selectedMapUrl);
    }

    private void loadMapList()
    {
        LoadingIndicator.SetActive(true);

        if (UserInfo.CurrentUser != null && UserInfo.CurrentUser != UserInfo.UnknownUser)
        {
            //var mapDtos = await WebAPI.Maps();
            //TODO: hook this up
        }

        var list = new[] {
            new ListItemDto() { Text = "Public Map", Value = "/public-map" }
        };

        MapsDropDownBox.SetItems(list);

        LoadingIndicator.SetActive(false);
    }

    private async void populateMap(string mapUrl)
    {
        bool isAbsUrl = mapUrl.Contains("://");
        string absUrl = mapUrl;

        if (!isAbsUrl && DestinationPresenter.Instance.CurrentUrl != null)
        {
            absUrl = mapUrl.TransformRelativeUrlToAbsolute(DestinationPresenter.Instance.CurrentUrl);
            isAbsUrl = true;
        }

        if(isAbsUrl) await DocumentManager.LoadDocumentIntoContainer(absUrl, MapContents.transform);
    }
}