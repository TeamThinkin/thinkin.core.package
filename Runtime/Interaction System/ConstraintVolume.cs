using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintVolume : MonoBehaviour
{
    [SerializeField] private Transform _volumeReference;
    public Transform VolumeReference => _volumeReference;
}
