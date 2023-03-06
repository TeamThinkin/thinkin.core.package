using System.Collections;
using UnityEngine;


public interface IKeyboard
{
    EditableText Text { get; }
    IFocusItem CurrentFocusItem { get; }

    void ShowForInput(IFocusItem Item);
    void Close();
}