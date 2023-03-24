using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NewUserDialogController : MonoBehaviour
{
    [SerializeField] private InviteCodeEntryPanel InviteCodeEntryPanel;
    [SerializeField] private InviteDetailsPanel InviteDetailsPanel;
    [SerializeField] private TabPanel IntroPanel;
    [SerializeField] private GameObject IntroPanelBackButton;

    private const float transitionDuration = 0.1f;

    private TabPanel currentPanel;

    public void PromptForInvite()
    {
        InviteCodeEntryPanel.gameObject.SetActive(false);
        InviteDetailsPanel.gameObject.SetActive(false);
        IntroPanel.gameObject.SetActive(false);
        IntroPanelBackButton.SetActive(true);
        gameObject.SetActive(true);

        _ = showPanel(InviteCodeEntryPanel);
    }

    public void ShowIntroPanel()
    {
        InviteCodeEntryPanel.gameObject.SetActive(false);
        InviteDetailsPanel.gameObject.SetActive(false);
        IntroPanel.gameObject.SetActive(false);
        IntroPanelBackButton.SetActive(false);
        gameObject.SetActive(true);

        _ = showPanel(IntroPanel);
    }

    public void CloseDialog()
    {
        gameObject.SetActive(false);
    }

    public async Task NextPanel()
    {
        await showPanel(IntroPanel);
    }

    public async Task PrevPanel()
    {
        if (currentPanel == InviteDetailsPanel)
            await showPanel(InviteCodeEntryPanel);
        else if (currentPanel == IntroPanel)
            await showPanel(InviteDetailsPanel.HasDetails ? InviteDetailsPanel : InviteCodeEntryPanel);
    }

    public async Task ShowInviteDetails(InviteDto InviteDetailsDto)
    {
        InviteCodeEntryPanel.gameObject.SetActive(false);
        InviteDetailsPanel.SetDetails(InviteDetailsDto);
        await showPanel(InviteDetailsPanel);
    }

    private async Task showPanel(TabPanel panel)
    {
        if (currentPanel == panel) return;

        _ = hidePanel();

        currentPanel = panel;
        currentPanel.gameObject.SetActive(true);

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

        panel.gameObject.SetActive(false);
    }
}
