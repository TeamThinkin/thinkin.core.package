using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TabletOptions", menuName = "ScriptableObjects/Tablet Options")]
public class TabletOptions : ScriptableObject
{
    public TabPanel[] Panels;
}
