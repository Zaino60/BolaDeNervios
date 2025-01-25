using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _target;
    [Header("Values")]
    [SerializeField] float _zOffset;

    Vector3 _offset;

    private void Start()
    {
        _offset = transform.position + Vector3.forward * _zOffset;
        if (!_target) _target = FindObjectOfType<Hamster>().transform;
    }

    private void LateUpdate()
    {
        transform.position = _target.position + _offset;
    }
}
