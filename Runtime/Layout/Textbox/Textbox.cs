using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textbox : MonoBehaviour, IFocusItem, IHandlePointerEvent
{
    [SerializeField] private TMPro.TMP_Text Label;
    [SerializeField] private GameObject CaretIndicator;

    public event Action<Textbox> Changed;

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

    private void Start()
    {
        CaretIndicator.SetActive(false);
    }

    protected void OnDestroy()
    {
        Keyboard.Text.ValueChanged -= Text_ValueChanged;
    }

    protected void OnDisable()
    {
        #pragma warning disable CS0253 // Possible unintended reference comparison; right hand side needs cast
        if (this == Keyboard.CurrentFocusItem)
            Keyboard.Close();
        #pragma warning restore CS0253 // Possible unintended reference comparison; right hand side needs cast
    }

    private void OnInteractionStart()
    {
        Keyboard.ShowForInput(this);
    }

    public void OnTriggerStart(IUIPointer Sender, RaycastHit RayInfo)
    {
        OnInteractionStart();
    }

    private void Text_ValueChanged(EditableText keyboardText)
    {
        Text = keyboardText.Value;
        updateCaretPosition();
    }

    public void KeyPressed(string Key, SpecialKeyboardKey SpecialKey)
    {
        switch (SpecialKey)
        {
            case SpecialKeyboardKey.Backspace:
                Text = Text.Substring(0, Text.Length - 1);
                break;
            case SpecialKeyboardKey.None:
                Text += Key;
                break;
        }
    }

    public void OnFocusStart()
    {
        Debug.Log("Focus start: " + gameObject.name);
        Keyboard.Text.Set(Text);
        Keyboard.Text.ValueChanged += Text_ValueChanged;
        updateCaretPosition();
        CaretIndicator.SetActive(true);
    }

    public void OnFocusEnd()
    {
        Debug.Log("Focus end: " + gameObject.name);
        Keyboard.Text.ValueChanged -= Text_ValueChanged;
        CaretIndicator.SetActive(false);
    }

    private void updateCaretPosition()
    {
        var textInfo = Label.GetTextInfo(Label.text);
        Vector3 position;
        if(Keyboard.Text.CaretPosition >= Label.text.Length)
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
            var charInfo = textInfo.characterInfo[Keyboard.Text.CaretPosition];
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
