using System.Collections;
using UnityEngine;

public interface IKeyboard
{
    void SetInput(IFocusItem Item);
    void Close();

    event System.Action<char> OnCharacterKeyPressed;
    event System.Action<KeyCode> OnCommandKeyPressed;
}