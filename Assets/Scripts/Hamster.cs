using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _speed = .25f;
    [SerializeField] float _fallDamage = 5f;
    [SerializeField] float _falYThreshold = -5f;
    [Header("References")]
    [SerializeField] Rigidbody _rb;

    Vector3 _checkpoint;

    void Start()
    {
        AudioManager.instance.Play("BubblePop");
        TakeDamage(1f);
        _checkpoint = transform.position;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.1)) TakeDamage(1f);
#if UNITY_EDITOR
        //Cheatcodes, solo funcionan en el editor
        if (Input.GetKeyDown(KeyCode.Return)) UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.Space)) _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
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

        if (transform.position.y <= _falYThreshold) PlayerFellOutOfMap();
    }

    void PlayerFellOutOfMap()
    {
        TakeDamage(_fallDamage);
        transform.position = _checkpoint;
    }

    public void TakeDamage(float damage)
    {
        LevelManager.Instance.AnxietyTimer -= damage;
        Debug.Log($"el hamster perdió {damage} de sanidad! Nueva sanidad {LevelManager.Instance.AnxietyTimer}");
        //if (LevelManager.Instance.AnxietyTimer <= 0) LevelManager.Instance.GameOver();
    }

    public void Heal(float amount)
    {
        LevelManager.Instance.AnxietyTimer = Mathf.Clamp(LevelManager.Instance.AnxietyTimer + amount, 0, LevelManager.Instance.LvlTimer);
        Debug.Log($"el hamster ganó {amount} sanidad! Nueva sanidad {LevelManager.Instance.AnxietyTimer}");
    }

    public void SetCheckpoint(Vector3 position)
    {
        _checkpoint = position;
    }
}
