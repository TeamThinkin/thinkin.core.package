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

        showPanel(InviteCodeEntryPanel);
    }

    public void ShowIntroPanel()
    {
        InviteCodeEntryPanel.gameObject.SetActive(false);
        InviteDetailsPanel.gameObject.SetActive(false);
        IntroPanel.gameObject.SetActive(false);
        IntroPanelBackButton.SetActive(false);
        gameObject.SetActive(true);
        
        showPanel(IntroPanel);
    }

    public void CloseDialog()
    {
        gameObject.SetActive(false);
    }

    public void NextPanel()
    {
        showPanel(IntroPanel);
    }

    public void PrevPanel()
    {
        if (currentPanel == InviteDetailsPanel)
            showPanel(InviteCodeEntryPanel);
        else if (currentPanel == IntroPanel)
            showPanel(InviteDetailsPanel.HasDetails ? InviteDetailsPanel : InviteCodeEntryPanel);
    }

    public void ShowInviteDetails(InviteDto InviteDetailsDto)
    {
        InviteCodeEntryPanel.gameObject.SetActive(false);
        InviteDetailsPanel.SetDetails(InviteDetailsDto);
        showPanel(InviteDetailsPanel);
    }

    private async Task showPanel(TabPanel panel)
    {
        if (currentPanel == panel) return;

        hidePanel();

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
