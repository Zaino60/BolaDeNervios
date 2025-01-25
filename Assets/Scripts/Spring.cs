using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _jumpForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hamster>())
        {
            Debug.Log("boing");
            other.GetComponent<Rigidbody>().AddForce(transform.up * _jumpForce);
        }
    }
}
