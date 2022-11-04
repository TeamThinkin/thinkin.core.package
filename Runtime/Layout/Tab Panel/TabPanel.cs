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

    protected virtual void OnShow() { }
    protected virtual void OnHide() { }

    protected virtual void Start()
    {
        OnShow();
    }
}
