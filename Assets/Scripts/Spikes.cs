using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _damage;
    [SerializeField] float _knockbackForce;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Hamster>())
        {
            Debug.Log("colisioné con hamster");
            other.gameObject.GetComponent<Hamster>().TakeDamage(_damage);
            other.gameObject.GetComponent<Rigidbody>().AddForce(transform.up * _knockbackForce, ForceMode.Impulse);
        }
    }
}
