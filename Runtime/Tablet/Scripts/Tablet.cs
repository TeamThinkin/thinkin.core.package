using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    [SerializeField] private ElementPresenterBase MenuContentContainer;
    [SerializeField] private ElementPresenterBase PanelContainer;
    [SerializeField] private TMPro.TMP_Text BuildLabel;
    [SerializeField] private Sprite DefaultPanelIconSprite;
    [SerializeField] private ButtonInteractable MenuButtonPrefab;

    private TabPanel currentPanel;
    private const float transitionDuration = 0.1f;
    private List<ButtonInteractable> tabButtons = new List<ButtonInteractable>();

    private void Start()
    {
        //AppSceneManager.OnEnvironmentUnloaded += AppSceneManager_OnEnvironmentUnloaded;

        //BuildLabel.text = Application.version + ", " + GeneratedInfo.BundleVersionCode;//TODO: commented out during the Package refactor
        loadContentPanelButtons();
        showPanel(tabButtons[0].Key as TabPanel);
    }

    private void loadContentPanelButtons()
    {
        tabButtons.Clear();
        foreach (var panel in AppControllerBase.Instance.TabletSettings.Panels)
        {
            var button = Instantiate(MenuButtonPrefab).GetComponent<ButtonInteractable>();
            button.gameObject.name = "Button " + panel.DisplayName;
            button.SpriteRenderer.sprite = panel.IconSprite != null ? panel.IconSprite : DefaultPanelIconSprite;
            button.transform.SetParent(MenuContentContainer.SceneChildrenContainer.transform, false);
            button.transform.Reset();
            button.Key = panel;
            button.OnInteractionEvent += Button_OnInteractionEvent;
            tabButtons.Add(button);
        }

        var menuRoot = MenuContentContainer.GetRootParent();
        MenuContentContainer.GetRootParent().ExecuteLayout();
    }

    private async Task showPanel(TabPanel panelPrefab)
    {
        if (currentPanel != null && panelPrefab.DisplayName == currentPanel.DisplayName) return;

        hidePanel();

        currentPanel = Instantiate(panelPrefab);
        currentPanel.transform.SetParent(PanelContainer.SceneChildrenContainer.transform);
        currentPanel.transform.Reset();
        PanelContainer.ExecuteLayout();

        await AnimationHelper.StartAnimation(this, transitionDuration, 0, 1, t =>
        {
            currentPanel.transform.localScale = Vector3.one * t;
        });
    }

    private async Task hidePanel()
    {
        if (currentPanel == null) return;

        var panel = currentPanel;
        currentPanel = null;

        await AnimationHelper.StartAnimation(this, transitionDuration, 1, 0, t =>
        {
            panel.transform.localScale = Vector3.one * t;
        });

        Destroy(panel.gameObject);
    }

    private void Button_OnInteractionEvent(ButtonInteractable sender)
    {
        showPanel(sender.Key as TabPanel);
    }

    //private void OnDestroy()
    //{
    //    //TODO: commented out during the Package refactor
    //    //if (networkSync != null)
    //    //{
    //    //    Normal.Realtime.Realtime.Destroy(networkSync.gameObject);
    //    //}

    //    AppSceneManager.OnEnvironmentUnloaded -= AppSceneManager_OnEnvironmentUnloaded;
    //}

    //private void createNetworkSync()
    //{
    //    //TODO: commented out during the Package refactor
    //    //if (!TelepresenceRoomManager.Instance.IsConnected) return;

    //    //networkSync = Normal.Realtime.Realtime.Instantiate("Tablet (Remote)", Normal.Realtime.Realtime.InstantiateOptions.defaults).GetComponent<TabletNetworkSync>();
    //    //networkSync.SetSource(this);
    //}

    //private void AppSceneManager_OnEnvironmentUnloaded()
    //{
    //    //Destroy(this.gameObject); //TODO: this was commented out during dev, but should be restored when complete
    //}

}
