using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private ElementPresenterBase MenuContentContainer;
    [SerializeField] private TMPro.TMP_Text BuildLabel;
    [SerializeField] private Sprite DefaultPanelIconSprite;

    [SerializeField] private ButtonInteractable MenuButtonPrefab;
    [SerializeField] private Transform PanelContainer;

    //TODO: commented out during the Package refactor
    //private TabletNetworkSync networkSync;

    private void Start()
    {
        createNetworkSync();
        AppSceneManager.OnEnvironmentUnloaded += AppSceneManager_OnEnvironmentUnloaded;

        loadContentPanels();
        //BuildLabel.text = Application.version + ", " + GeneratedInfo.BundleVersionCode;//TODO: commented out during the Package refactor
        var menuRoot = MenuContentContainer.GetRootParent();
        MenuContentContainer.GetRootParent().ExecuteLayout();

    }

    private void loadContentPanels()
    {
        foreach (var panel in AppControllerBase.Instance.TabletSettings.Panels)
        {
            var button = Instantiate(MenuButtonPrefab).GetComponent<ButtonInteractable>();
            button.gameObject.name = "Button " + panel.DisplayName;
            button.SpriteRenderer.sprite = panel.IconSprite != null ? panel.IconSprite : DefaultPanelIconSprite;
            button.transform.SetParent(MenuContentContainer.SceneChildrenContainer.transform, false);
            button.transform.Reset();
            button.Key = panel;
            button.OnInteractionEvent += Button_OnInteractionEvent;
        }
    }

    private void Button_OnInteractionEvent(ButtonInteractable obj)
    {
        Debug.Log("Button clicked: " + obj.gameObject.name, obj.gameObject);
    }

    private void OnDestroy()
    {
        //TODO: commented out during the Package refactor
        //if (networkSync != null)
        //{
        //    Normal.Realtime.Realtime.Destroy(networkSync.gameObject);
        //}

        AppSceneManager.OnEnvironmentUnloaded -= AppSceneManager_OnEnvironmentUnloaded;
    }

    private void createNetworkSync()
    {
        //TODO: commented out during the Package refactor
        //if (!TelepresenceRoomManager.Instance.IsConnected) return;

        //networkSync = Normal.Realtime.Realtime.Instantiate("Tablet (Remote)", Normal.Realtime.Realtime.InstantiateOptions.defaults).GetComponent<TabletNetworkSync>();
        //networkSync.SetSource(this);
    }

    private void AppSceneManager_OnEnvironmentUnloaded()
    {
        //Destroy(this.gameObject); //TODO: this was commented out during dev, but should be restored when complete
    }

}
