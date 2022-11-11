using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractable : MonoBehaviour, IHandlePointerEvent
{
    public object Key;
    public bool IsToggle;
    public bool IsPhysicalHandInteractionsDisabled;

    [SerializeField] protected Transform Visuals;
    [SerializeField] protected bool StartPressed = false;
    [SerializeField] protected Vector3 PressOffset = new Vector3(0, 0, 0.05f);
    [SerializeField] protected TMPro.TMP_Text Label;
    [SerializeField] protected AudioClip PressedAudio;
    [SerializeField] protected AudioClip UnpressedAudio;
    [SerializeField] protected AudioSource _audioPlayer;
    [SerializeField] protected Material ActiveMaterial;
    [SerializeField] protected Renderer BackgroundRenderer;
    [SerializeField] protected SpriteRenderer _SpriteRenderer;

    public SpriteRenderer SpriteRenderer => _SpriteRenderer;

    public event Action<ButtonInteractable> OnInteractionEvent;
    public UnityEvent OnInteractionUnityEvent;

    public event Action<ButtonInteractable> OnPressedEvent;
    public UnityEvent OnPressedUnityEvent;

    public event Action<ButtonInteractable> OnUnpressedEvent;
    public UnityEvent OnUnpressedUnityEvent;

    protected Material InactiveMaterial;
    protected bool isHoverScaleFeedbackEnabled = true;

    public AudioSource AudioPlayer => _audioPlayer;

    private bool _isPressed = false;
    public bool IsPressed
    {
        get { return _isPressed; }
        set
        {
            _isPressed = value;
            updateState();
        }
    }

    public string Text
    {
        get { return Label?.text; }
        set { if(Label != null) Label.text = value; }
    }

    protected virtual void Awake()
    {
        if (BackgroundRenderer != null)
        {
            InactiveMaterial = BackgroundRenderer.sharedMaterial;
            updateState();
        }
    }

    protected virtual void  onInteractionStart()
    {
        if (IsToggle)
        {
            if (!_isPressed)
                Pressed();
            else if (_isPressed)
                Released();
        }
        else if (!_isPressed) Pressed();

        OnInteractionEvent?.Invoke(this);
        OnInteractionUnityEvent.Invoke();
    }

    protected virtual void onInteractionEnd()
    {
        if (_isPressed && !IsToggle) Released();
    }

    protected virtual void Pressed()
    {
        if (!_isPressed && Visuals != null) Visuals.localPosition = PressOffset;

        IsPressed = true;
        AudioPlayer?.PlayOneShot(PressedAudio);
        OnPressedEvent?.Invoke(this);
        OnPressedUnityEvent?.Invoke();
    }

    protected virtual void Released()
    {
        if (gameObject == null) return;
        
        if (_isPressed && Visuals != null) Visuals.localPosition = Vector3.zero;

        IsPressed = false;
        AudioPlayer?.PlayOneShot(UnpressedAudio);
        OnUnpressedEvent?.Invoke(this);
        OnUnpressedUnityEvent.Invoke();
    }

    private void updateState()
    {
        if (Visuals == null || Visuals.gameObject == null) return;

        Visuals.localPosition = IsPressed ? PressOffset : Vector3.zero;
        if (BackgroundRenderer != null) BackgroundRenderer.sharedMaterial = _isPressed ? ActiveMaterial : InactiveMaterial;
    }

    #region -- Pointer Events --
    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        onInteractionStart();
    }

    public void OnTriggerEnd(IUIPointer Sender)
    {
        onInteractionEnd();
    }

    virtual public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        if(isHoverScaleFeedbackEnabled) Visuals.transform.localScale = Vector3.one * 1.1f;
    }

    virtual public void OnHoverEnd(IUIPointer Sender)
    {
        if (isHoverScaleFeedbackEnabled) Visuals.transform.localScale = Vector3.one;
    }

    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnGripEnd(IUIPointer Sender)
    {
    }
    #endregion
}
