using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _damage;
    [SerializeField] float _knockbackForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            Debug.Log("colisioné con hamster");
            collision.gameObject.GetComponent<Hamster>().TakeDamage(_damage);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * _knockbackForce);
        }
    }
}
