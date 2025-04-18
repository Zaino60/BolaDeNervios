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
    [SerializeField] float _lookUpSpeed = .25f;

    //Vector3 _offset;
    Quaternion _spawnRotation;

    public float distance = 10.0f;
    private float currentX = 50.0f;
    private float currentY = -90.0f;
    public float sensivity = 150.0f;

    bool _onCinematic;
    public bool CloseLookup;

    private void Start()
    {
        //_offset = transform.position + Vector3.forward * _zOffset;
        if (!_target) _target = FindObjectOfType<Hamster>().transform;
    }

    //private void LateUpdate()
    //{
    //    transform.position = _target.position + _offset;
    //}

    void LateUpdate()
    {
        if(!_onCinematic)
        {
            currentY += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;

            Vector3 Direction = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
            transform.position = _target.position + rotation * Direction;

            transform.LookAt(_target.position);
        }
        if (CloseLookup) FrontCloseUp();
    }

    void FrontCloseUp()
    {
        _onCinematic = true;
        //_offset = Mathf.Lerp(_offset, 0f, Time.deltaTime * 5f * 2f);
        transform.position = Vector3.Slerp(transform.position, _target.transform.position + new Vector3(0f, 1.5f, 0f) + _target.transform.forward * 6f, Time.deltaTime * _lookUpSpeed * 4f);
        var target = Quaternion.LookRotation(new Vector3(_target.transform.position.x, _target.transform.position.y + 2f, _target.transform.position.z) - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * _lookUpSpeed * 8f);
    }

    public float GetSpawnRotationOffset() => _spawnRotation.eulerAngles.y;
}
