using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _speed = .25f;

    [Header("References")]
    [SerializeField] Rigidbody _rb;

    void Start()
    {
        AudioManager.instance.Play("BubblePop");
    }

    private void Update()
    {
#if UNITY_EDITOR
        //Cheatcodes, solo funcionan en el editor
        if (Input.GetKeyDown(KeyCode.Return)) UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //Con Enter restarteo la escena, para evitar estar poniendo y sacando play todo el tiempo
#endif
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal")).normalized;
        _rb.AddForce(dir * _speed);
    }
}
