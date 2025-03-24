using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    [SerializeField] float _windForce = 5f;

    private void OnTriggerStay(Collider other)
    {
        //if (other.GetComponent<Hamster>()) other.GetComponent<Rigidbody>().AddForce(transform.up * _windForce);
        other.GetComponent<Rigidbody>().AddForce(transform.up * _windForce);
    }
}
