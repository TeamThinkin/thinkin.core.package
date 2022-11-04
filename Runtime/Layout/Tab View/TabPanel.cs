using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabPanel : MonoBehaviour
{
    [SerializeField] private string _displayName;
    public string DisplayName => _displayName;

    [SerializeField] private Sprite _iconSprite;
    public Sprite IconSprite => _iconSprite;

    public TabView ParentTabView;

    private Coroutine animateScaleCoroutine;
    private Action animateScaleCallback;

    //public void ShowTab()
    //{
    //    ParentTabView.ShowTab(this);
    //}

    ///// <summary>
    ///// Called from the parent tab view
    ///// </summary>
    ///// <param name="Immediate"></param>
    //public void Show(bool Immediate = false)
    //{
    //    gameObject.SetActive(true);
    //    if (Immediate)
    //    {
    //        transform.localScale = Vector3.one;
    //    }
    //    else
    //    {
    //        gameObject.SetActive(true);
    //        startAnimateScale(1);
    //    }
    //    OnShow();
    //}



    //public void Hide(bool Immediate = false)
    //{
    //    if (Immediate)
    //    {
    //        transform.localScale = Vector3.zero;
    //        gameObject.SetActive(false);
    //        OnHide();
    //    }
    //    else
    //    {
    //        startAnimateScale(0, () =>
    //        {
    //            gameObject.SetActive(false);
    //            OnHide();
    //        });
    //    }
    //}

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

    protected virtual void Start()
    {
        OnShow();
    }

    //private void startAnimateScale(float targetScale, Action completeCallback = null)
    //{
    //    if (animateScaleCoroutine != null) StopCoroutine(animateScaleCoroutine);
    //    animateScaleCallback = completeCallback;
    //    StartCoroutine(animateScale(targetScale));

    //}
    //private IEnumerator animateScale(float targetScale)
    //{
    //    float duration = 0.25f;
    //    float elapsed = 0;
    //    float t;

    //    var startingValue = transform.localScale.x; //Note: assuming scale to be uniform

    //    while (elapsed <= duration)
    //    {
    //        t = elapsed / duration;

    //        transform.localScale = Mathf.Lerp(startingValue, targetScale, t) * Vector3.one;

    //        yield return null;
    //        elapsed += Time.deltaTime;
    //    }

    //    transform.localScale = targetScale * Vector3.one;
    //    animateScaleCoroutine = null;
    //    animateScaleCallback?.Invoke();
    //    animateScaleCallback = null;
    //}
}
