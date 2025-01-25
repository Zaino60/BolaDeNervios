using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _target;
    [Header("Values")]
    [SerializeField] float _zOffset;

    Vector3 _offset;

    public float distance = 10.0f;
    private float currentX = 50.0f;
    private float currentY = -90.0f;
    public float sensivity = 150.0f;

    private void Start()
    {
        _offset = transform.position + Vector3.forward * _zOffset;
        if (!_target) _target = FindObjectOfType<Hamster>().transform;
    }

    //private void LateUpdate()
    //{
    //    transform.position = _target.position + _offset;
    //}

    void LateUpdate()
    {

        currentY += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
        transform.position = _target.position + rotation * Direction;

        transform.LookAt(_target.position);


    }
}
