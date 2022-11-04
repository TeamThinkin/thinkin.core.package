using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPanel : TabPanel
{
    [SerializeField] private DropDownBox MapsDropDownBox;
    [SerializeField] private GameObject MapContents;
    [SerializeField] private GameObject LoadingIndicator;
    
    private string selectedMapUrl;

    protected override void OnShow()
    {
        base.OnShow();

        MapsDropDownBox.SelectedItemChanged += MapsDropDownBox_SelectedItemChanged;
        loadMapList();
        UserInfo.OnCurrentUserChanged += UserInfo_OnCurrentUserChanged;

        Debug.Log("Listening for url change");
        DestinationPresenter.UrlChanged += DestinationPresenter_UrlChanged;
    }

    protected override void OnHide()
    {
        base.OnHide();
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;
        DestinationPresenter.UrlChanged -= DestinationPresenter_UrlChanged;
    }

    private void OnDestroy()
    {
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;
        MapsDropDownBox.SelectedItemChanged -= MapsDropDownBox_SelectedItemChanged;
    }

    //public async void NavigateHome()
    //{
    //    await DestinationPresenter.Instance.DisplayUrl(UserInfo.CurrentUser.HomeRoomUrl);
    //}

    private void UserInfo_OnCurrentUserChanged(UserInfo obj)
    {
        loadMapList();
    }


    private void DestinationPresenter_UrlChanged(string obj)
    {
        loadMapList();
    }

    private void MapsDropDownBox_SelectedItemChanged(ListItemDto selectedItem)
    {
        string url = selectedItem.Value as string;
        if (selectedMapUrl == url) return;

        selectedMapUrl = url;
        populateMap(selectedMapUrl);
    }

    private async void loadMapList()
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