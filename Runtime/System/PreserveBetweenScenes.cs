using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreserveBetweenScenes : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
