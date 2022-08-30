using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInsepector : MonoBehaviour
{
    [SerializeField] private string Url;
    [SerializeField] private DocumentInspector DocInspector;

    void Start()
    {
        DocInspector.DisplayUrl(Url);    
    }
}
