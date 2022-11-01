using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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

    //[Space]
    //[SerializeField] protected UnityHandEvent OnPressed;
    //[SerializeField] protected UnityHandEvent OnUnpressed;

    public event Action<ButtonInteractable> OnInteractionEvent;
    public event Action<ButtonInteractable> OnPressedEvent;
    public event Action<ButtonInteractable> OnUnpressedEvent;

    protected Material InactiveMaterial;

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
        //OnPressed?.Invoke();
        OnPressedEvent?.Invoke(this);
    }

    protected virtual void Released()
    {
        if (_isPressed && Visuals != null) Visuals.localPosition = Vector3.zero;

        IsPressed = false;
        AudioPlayer?.PlayOneShot(UnpressedAudio);
        //OnUnpressed?.Invoke();
        OnUnpressedEvent?.Invoke(this);
    }

    private void updateState()
    {
        Visuals.localPosition = IsPressed ? PressOffset : Vector3.zero;
        if (BackgroundRenderer != null) BackgroundRenderer.sharedMaterial = _isPressed ? ActiveMaterial : InactiveMaterial;
    }

    //#region -- Touch Events --
    //protected override void OnTouch(Hand hand, Collision collision)
    //{
    //    base.OnTouch(hand, collision);
    //    return;

    //    if (IsPhysicalHandInteractionsDisabled) return;
    //    if (!collision.InvolvesPrimaryFingerTip()) return; //Only accept input from pointer finger tips to hopefully filter out accidental touches

    //    onInteractionStart(hand);
    //}

    //protected override void OnUntouch(Hand hand, Collision collision)
    //{
    //    base.OnUntouch(hand, collision);
    //    return;

    //    if (IsPhysicalHandInteractionsDisabled) return;
    //    onInteractionEnd(hand);
    //}
    //#endregion

    #region -- Pointer Events --
    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        onInteractionStart();
    }

    public void OnTriggerEnd(IUIPointer Sender)
    {
        onInteractionEnd();
    }

    public void OnHoverStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnHoverEnd(IUIPointer Sender)
    {
    }

    public void OnGripStart(IUIPointer Sender, RaycastHit RayInfo)
    {
    }

    public void OnGripEnd(IUIPointer Sender)
    {
    }
    #endregion
}
