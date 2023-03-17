using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InviteDialogController : MonoBehaviour
{
    [SerializeField] private Textbox CodeTextbox1;
    [SerializeField] private Textbox CodeTextbox2;
    [SerializeField] private Textbox CodeTextbox3;
    [SerializeField] private GameObject CheckingIndicator;
    [SerializeField] private GameObject InvalidCodeIndicator;

    private int zeroCount2, zeroCount3;
    private int prevBoxLength2, prevBoxLength3;

    private void Awake()
    {
        CodeTextbox1.Changed += CodeTextbox1_Changed;
        CodeTextbox2.Changed += CodeTextbox2_Changed;
        CodeTextbox3.Changed += CodeTextbox3_Changed;
    }

    private void OnDestroy()
    {
        CodeTextbox1.Changed -= CodeTextbox1_Changed;
        CodeTextbox2.Changed -= CodeTextbox2_Changed;
        CodeTextbox3.Changed -= CodeTextbox3_Changed;
    }

    private void CodeTextbox1_Changed(Textbox textbox)
    {
        var text = textbox.Text.Trim();
        if(text != textbox.Text)
        {
            textbox.Text = text;
            return;
        }

        if (text.Length == 2) FocusManager.SetFocus(CodeTextbox2);

        checkCode();
    }

    private void CodeTextbox2_Changed(Textbox textbox)
    {
        var text = textbox.Text.Trim();
        if (text != textbox.Text)
        {
            prevBoxLength2 = textbox.Text.Length;
            textbox.Text = text;
            return;
        }

        if (textbox.Text.Length == 2) FocusManager.SetFocus(CodeTextbox3);

        if(textbox.Text.Length == 0 && prevBoxLength2 == 0)
        {
            CodeTextbox1.Text = CodeTextbox1.Text.Left(1);
            FocusManager.SetFocus(CodeTextbox1);
        }
        prevBoxLength2 = textbox.Text.Length;

        checkCode();
    }

    private void CodeTextbox3_Changed(Textbox textbox)
    {
        var text = textbox.Text.Trim();
        if (text != textbox.Text)
        {
            prevBoxLength3 = textbox.Text.Length;
            textbox.Text = text;
            return;
        }

        if (textbox.Text.Length == 0 && prevBoxLength3 == 0)
        {
            CodeTextbox2.Text = CodeTextbox2.Text.Left(1);
            FocusManager.SetFocus(CodeTextbox2);
        }
        prevBoxLength3 = textbox.Text.Length;

        checkCode();
    }

    private async void checkCode()
    {
        InvalidCodeIndicator.SetActive(false);

        if (CodeTextbox1.Text.Length != 2 || CodeTextbox2.Text.Length != 2 || CodeTextbox3.Text.Length != 2) return;

        CheckingIndicator.SetActive(true);
        string code = CodeTextbox1.Text + "-" + CodeTextbox2.Text + "-" + CodeTextbox3.Text;
        Debug.Log("Checking code: " + code);
        await System.Threading.Tasks.Task.Delay(1000);
        CheckingIndicator.SetActive(false);

        InvalidCodeIndicator.SetActive(true);
    }

    public void ShowDialog()
    {
        CheckingIndicator.SetActive(false);
        InvalidCodeIndicator.SetActive(false);
        gameObject.SetActive(true);
        FocusManager.SetFocus(CodeTextbox1);
    }

}
