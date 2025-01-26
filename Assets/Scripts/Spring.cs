using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _jumpForce;

    AudioSource AS;
    [SerializeField] AudioClip[] sounds;

    Animator anim;

    private void Start()
    {
        AS = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hamster>())
        {
            Debug.Log("boing");
            other.GetComponent<Rigidbody>().AddForce(transform.up * _jumpForce);

            int randomNum = Random.Range(0, sounds.Length);
            AS.clip = sounds[randomNum];
            if (AS.isPlaying)
            {
                AS.Stop();
            }
            AS.Play();

            anim.Play(Animator.StringToHash("SpringBounce"));
        }
    }
}
