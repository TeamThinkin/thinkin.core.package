using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textbox : MonoBehaviour, IFocusItem, IHandlePointerEvent
{
    [SerializeField] private TMPro.TMP_Text Label;
    [SerializeField] private GameObject CaretIndicator;
    [SerializeField] private GameObject Highlight;

    public event Action<Textbox> Changed;
    public bool IsMultiline;

    private IKeyboard Keyboard => AppControllerBase.Instance.Keyboard;

    public string Text
    {
        get { return Label.text; }
        set 
        {
            if (value == Label.text) return;
            Label.text = value;
            Changed?.Invoke(this);
        }
    }

    private int _caretPosition;
    public int CaretPosition
    {
        get { return _caretPosition; }
        set { _caretPosition = Mathf.Clamp(value, 0, Label.text.Length); }
    }

    private bool _isFocused;
    public bool IsFocused 
    {
        get { return _isFocused; }
        private set
        {
            _isFocused = value;
            Highlight.SetActive(value);
        }
    }

    private void Start()
    {
        CaretIndicator.SetActive(false);
        IsFocused = false;
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
        Keyboard.SetInput(this);
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        OnInteractionStart();
    }

    public void OnFocusStart()
    {
        if (IsFocused) return;
        Keyboard.OnCharacterKeyPressed += Keyboard_OnKeyPressed;
        Keyboard.OnCommandKeyPressed += Keyboard_OnCommandKeyPressed;
        CaretPosition = Text.Length;
        updateVisualCaretPosition();
        CaretIndicator.SetActive(true);
        IsFocused = true;
    }

    public void OnFocusEnd()
    {
        Keyboard.Close();
        Keyboard.OnCharacterKeyPressed -= Keyboard_OnKeyPressed;
        Keyboard.OnCommandKeyPressed -= Keyboard_OnCommandKeyPressed;
        CaretIndicator.SetActive(false);
        IsFocused = false;
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
                if (Text.Length == 0 || CaretPosition == 0) break;
                Text = Text.Substring(0, CaretPosition - 1) + Text.Substring(CaretPosition);
                CaretPosition--;
                updateVisualCaretPosition();
                break;
            case KeyCode.Delete:
                if (CaretPosition >= Text.Length || Text.Length == 0) break;
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
            }
            else
            {
                position = Label.transform.position;
            }
        }
        else
        {
            var charInfo = textInfo.characterInfo[CaretPosition];
            position = Label.transform.TransformPoint(charInfo.bottomLeft);
        }

        CaretIndicator.transform.position = position;
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
