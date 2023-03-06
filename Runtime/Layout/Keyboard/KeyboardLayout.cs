using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keyboard Layout", menuName = "ScriptableObjects/New Keyboard Layout")]
public class KeyboardLayout : ScriptableObject
{
    public GameObject KeyPrefab;
    public KeyboardRow[] Rows;
}

[Serializable]
public class KeyboardRow
{
    public KeyboardKey[] Keys;
}

[Serializable]
public class KeyboardKey
{
    public string MainKey;
    public string SecondaryKey;
    public string DisplayText;
    public int Width = 1;
    public SpecialKeyboardKey Special;
}

public enum SpecialKeyboardKey
{
    None,
    Backspace,
    Delete,
    Shift,
    Close
}
