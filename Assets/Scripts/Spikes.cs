using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _damage;
    [SerializeField] float _knockbackForce;
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Hamster>())
    //    {
    //        Debug.Log("colisioné con hamster");
    //        other.gameObject.GetComponent<Hamster>().TakeDamage(_damage);
    //        other.gameObject.GetComponent<Rigidbody>().AddForce(.oth * _knockbackForce, ForceMode.Impulse);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Hamster>())
        {
            Debug.Log("colisioné con hamster");
            collision.gameObject.GetComponent<Hamster>().TakeDamage(_damage);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(-collision.contacts[0].normal * _knockbackForce, ForceMode.Impulse);
        }
    }
}
