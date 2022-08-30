using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] Animator TransitionAnimator;
    [SerializeField] MeshRenderer itemRenderer;

    public event System.Action OnSceneHidden;

    public static TransitionController Instance { get; private set; }

    private System.Action onSceneHiddenCallback;
    private bool isHidePending;

    private void Awake()
    {
        Instance = this;
    }

    private Task hideSceneTask;
    public async Task HideScene()
    {
        if (TransitionAnimator.GetBool("IsSolid")) return;

        isHidePending = true;
        //gameObject.SetActive(true);
        itemRenderer.enabled = true;
        TransitionAnimator.SetBool("IsSolid", true);

        await Task.Run(() =>
        {
            while(isHidePending)
            {
                Thread.Sleep(1);
            }
        });
    }

    public void HideScene(System.Action OnSceneHiddenCallback = null)
    {
        gameObject.SetActive(true);
        this.onSceneHiddenCallback = OnSceneHiddenCallback;

        TransitionAnimator.SetBool("IsSolid", true);
    }

    public void RevealScene()
    {
        TransitionAnimator.SetBool("IsSolid", false);
    }

    public void OnSolidfiedEvent() //Called from Animation Event
    {
        isHidePending = false;
        onSceneHiddenCallback?.Invoke();
        OnSceneHidden?.Invoke();
    }

    public void OnDissolvedEvent() //Called from Animation Event
    {
        //gameObject.SetActive(false);
        itemRenderer.enabled = false;
    }
}
