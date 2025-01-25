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
    [SerializeField] float _rotSpeed;
    [Header("References")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _hamsterMesh;
    [SerializeField] Animator _anim;

    Vector3 _checkpoint;
    Vector3 _lastValidDir;

    float _xAxis, _zAxis;

    void Start()
    {
        AudioManager.instance.Play("BubblePop");
        TakeDamage(1f);
        _checkpoint = transform.position;
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");

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
        _anim.SetBool("Running", (_zAxis != 0 || _xAxis != 0));
        Debug.Log($"Running: {(_zAxis != 0 || _xAxis != 0)}");
        Movement();
        MeshRotation();
    }

    void MeshRotation()
    {
        //Vector3 Dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized; //Creo vector dirección determinado por los WASD que estoy apretando
        Vector3 Dir = new Vector3(-_zAxis, 0f, _xAxis).normalized; //Creo vector dirección determinado por los WASD que estoy apretando

        if (Dir != Vector3.zero) _lastValidDir = Dir;
        //if (Dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), Time.fixedDeltaTime * _rotSpeed); //anterior
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_lastValidDir), Time.fixedDeltaTime * _rotSpeed);
    }

    void Movement()
    {
        Vector3 dir = new Vector3(-_zAxis, 0f, _xAxis).normalized;
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
        Debug.Log($"el hamster perdió {damage} de sanidad! Nueva sanidad {LevelManager.Instance.AnxietyTimer}");
        //if (LevelManager.Instance.AnxietyTimer <= 0) LevelManager.Instance.GameOver();

        if (LevelManager.Instance.AnxietyTimer > 0)
        {
            LevelManager.Instance.AnxietyTimer -= damage;
        }
        else
        {
            LevelManager.Instance.currentExtraTimerBeforeKill -= damage;
        }
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
