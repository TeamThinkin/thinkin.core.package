using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textbox : MonoBehaviour, IFocusItem, IHandlePointerEvent
{
    [SerializeField] private TMPro.TMP_Text Label;
    [SerializeField] private GameObject CaretIndicator;

    public event Action<Textbox> Changed;
    public bool IsMultiline;
    public string PromptText;
    public bool IsLengthLimited;
    public int MaxLength;

    
    private Renderer caretRendered;

    private IKeyboard Keyboard => AppControllerBase.Instance.Keyboard;

    public bool IsFocused { get; private set; }

    private float? _defaultCaretPosition;
    private float defaultCaretPosition
    {
        get
        {
            if (!_defaultCaretPosition.HasValue) _defaultCaretPosition = CaretIndicator.transform.localPosition.y;
            return _defaultCaretPosition.Value;
        }
    }


    private string _text = "";
    public string Text
    {
        get { return _text; }
        set 
        {
            if (value == Label.text) return;
            if (value == null) value = "";

            _text = IsLengthLimited ? value.Left(MaxLength) : value;
            Label.text = _text;
            Changed?.Invoke(this);
        }
    }

    private int _caretPosition;
    public int CaretPosition
    {
        get { return _caretPosition; }
        set { _caretPosition = Mathf.Clamp(value, 0, Label.text.Length); }
    }

    private void Awake()
    {
        caretRendered = CaretIndicator.GetComponentInChildren<Renderer>();
    }

    private void blinkCaret()
    {
        caretRendered.enabled = !caretRendered.enabled;
    }

    private void Start()
    {
        if (!IsFocused)
        {
            Label.text = PromptText;
            CaretIndicator.SetActive(false);
        }
    }

    protected void OnDisable()
    {
        if (IsFocused)
        {
            OnFocusEnd();
            Keyboard.Close();
        }
    }

    private void OnInteractionStart()
    {
        FocusManager.SetFocus(this);
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        OnInteractionStart();
    }

    public void OnFocusStart()
    {
        if (IsFocused) return;

        IsFocused = true;

        Keyboard.SetInput(this);
        Keyboard.OnCharacterKeyPressed += Keyboard_OnKeyPressed;
        Keyboard.OnCommandKeyPressed += Keyboard_OnCommandKeyPressed;

        Label.text = Text;
        CaretPosition = Text.Length;
        updateVisualCaretPosition();
        CaretIndicator.SetActive(true);
        InvokeRepeating("blinkCaret", 0.1f, 0.65f);

    }

    public void OnFocusEnd()
    {
        IsFocused = false;
        Keyboard.Close();
        Keyboard.OnCharacterKeyPressed -= Keyboard_OnKeyPressed;
        Keyboard.OnCommandKeyPressed -= Keyboard_OnCommandKeyPressed;
        CaretIndicator.SetActive(false);
        CancelInvoke("blinkCaret");
        if (Text.IsNullOrEmpty()) Label.text = PromptText;
    }

    private void Keyboard_OnKeyPressed(char character)
    {
        Text = Text.Substring(0, CaretPosition) + character + Text.Substring(CaretPosition);
        CaretPosition++;
        updateVisualCaretPosition();
    }

    private void Keyboard_OnCommandKeyPressed(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.KeypadEnter:
            case KeyCode.Return:
                if (!IsMultiline) break;
                Text = Text.Substring(0, CaretPosition) + '\n' + Text.Substring(CaretPosition);
                CaretPosition++;
                updateVisualCaretPosition();
                break;
            case KeyCode.Backspace:
                if (Text.Length == 0 || CaretPosition == 0)
                {
                    Changed?.Invoke(this);
                    break;
                }

                Text = Text.Substring(0, CaretPosition - 1) + Text.Substring(CaretPosition);
                CaretPosition--;
                updateVisualCaretPosition();
                break;
            case KeyCode.Delete:
                if (CaretPosition >= Text.Length || Text.Length == 0)
                {
                    Changed?.Invoke(this);
                    break;
                }

                Text = Text.Substring(0, CaretPosition) + Text.Substring(CaretPosition + 1);
                updateVisualCaretPosition();
                break;
            case KeyCode.LeftArrow:
                CaretPosition--;
                updateVisualCaretPosition();
                break;
            case KeyCode.RightArrow:
                CaretPosition++;
                updateVisualCaretPosition();
                break;
            case KeyCode.Home:
                CaretPosition = 0;
                updateVisualCaretPosition();
                break;
            case KeyCode.End:
                CaretPosition = Text.Length;
                updateVisualCaretPosition();
                break;
        }
    }

    private void updateVisualCaretPosition()
    {
        var textInfo = Label.GetTextInfo(Label.text);
        Vector3 position;
        if (CaretPosition >= Label.text.Length)
        {
            if (Label.text.Length > 0)
            {
                var charInfo = textInfo.characterInfo[Label.text.Length - 1];
                position = Label.transform.TransformPoint(charInfo.bottomRight);
                CaretIndicator.transform.position = position;
            }
            else
            {
                CaretIndicator.transform.localPosition = Vector3.up * defaultCaretPosition;
            }
        }
        else
        {
            var charInfo = textInfo.characterInfo[CaretPosition];
            position = Label.transform.TransformPoint(charInfo.bottomLeft);
            CaretIndicator.transform.position = position;
        }

        
    }

    #region -- Unused Pointer Events --
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

    public void OnTriggerEnd(IUIPointer Sender)
    {
    }
    #endregion
}
