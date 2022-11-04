using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapPanel : TabPanel
{
    [SerializeField] private DropDownBox MapsDropDownBox;
    //[SerializeField] private MapView MapContents;
    [SerializeField] private GameObject MapContents;
    //[SerializeField] private TabPanel SelectEnvironmentPanel;
    //[SerializeField] private NewMapDestinationWizard NewDestinationWizard;

    //public RegistryEntryDto SelectedMapDto { get; private set; }
    private string selectedMapUrl;

    protected override void OnShow()
    {
        Debug.Log("Map Panel show");
        base.OnShow();

        MapsDropDownBox.SelectedItemChanged += MapsDropDownBox_SelectedItemChanged;

        loadMapList();

        UserInfo.OnCurrentUserChanged += UserInfo_OnCurrentUserChanged;
    }


    protected override void OnHide()
    {
        base.OnHide();
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;//TODO: commented out during the Package refactor
    }

    private void OnDestroy()
    {
        UserInfo.OnCurrentUserChanged -= UserInfo_OnCurrentUserChanged;//TODO: commented out during the Package refactor
        MapsDropDownBox.SelectedItemChanged -= MapsDropDownBox_SelectedItemChanged;
    }

    public async void NavigateHome()
    {
        await DestinationPresenter.Instance.DisplayUrl(UserInfo.CurrentUser.HomeRoomUrl);
    }

    //public void NewDestinationButtonClicked()
    //{
    //    NewDestinationWizard.StartWizard();
    //}

    private void UserInfo_OnCurrentUserChanged(UserInfo obj)
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
        Debug.Log("Loading map list...");
        //var mapDtos = await WebAPI.Maps();
        //var list = mapDtos.Select(i => new ListItemDto() { Value = i, Text = i.DisplayName });

        if (UserInfo.CurrentUser != null && UserInfo.CurrentUser != UserInfo.UnknownUser)
        {

        }

        var list = new[] {
            new ListItemDto() { Text = "Public Map", Value = "/public-map" },
            new ListItemDto() { Text = "Another Map", Value = "/public-map" },
            new ListItemDto() { Text = "That Map", Value = "/public-map" }
        };

        MapsDropDownBox.SetItems(list);
    }

    private async void populateMap(string mapUrl)
    {
        var absUrl = mapUrl.TransformRelativeUrlToAbsolute(DestinationPresenter.Instance.CurrentUrl);
        Debug.Log("Populating map with url: " + absUrl);
        //await MapContents.LoadCollection(mapUrl);
        //DestinationPresenter.Instance.CurrentUrl.getsub
        await DocumentManager.LoadDocumentIntoContainer(absUrl, MapContents.transform);
    }
}