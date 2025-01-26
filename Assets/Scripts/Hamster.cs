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
    [SerializeField] float _minSlopeAngleToBoostSpeed = 15f;
    [SerializeField] float _slopeForce;
    [Header("References")]
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _hamsterMesh;
    [SerializeField] Animator _anim;

    Vector3 _checkpoint;
    Vector3 _lastValidDir;
    GameObject _camera;
    RaycastHit slopeHit;

    bool _dead;
    float _xAxis, _zAxis;

    //Sound
    [SerializeField] AudioClip[] sounds;
    AudioSource AS;


    private void Awake()
    {
        _camera = Camera.main.gameObject;
    }
    void Start()
    {
        _checkpoint = transform.position;
        if (LevelManager.Instance)
        {
            LevelManager.Instance.OnStateChange += StateAnimationCheck;
            LevelManager.Instance.OnGameOver += OnGameOver;
        }

        AS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");
        if (transform.position.y <= _falYThreshold) PlayerFellOutOfMap();

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
        if(!_dead) Movement();
    }

    void Movement()
    {
        //movimiento
        Vector3 dir = new Vector3(_xAxis, 0f, _zAxis).normalized;
        //Ajusto la corrección (rotacion del vector) en base a mi camara que puede estar rotada
        float offset = _camera.GetComponent<CameraController>().GetSpawnRotationOffset();
        Vector3 adjustedAngle = Quaternion.AngleAxis(_camera.transform.rotation.eulerAngles.y - offset, Vector3.up) * dir; 
        _rb.AddForce(adjustedAngle * _speed);

        //rotación
        if (adjustedAngle != Vector3.zero) _lastValidDir = adjustedAngle;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_lastValidDir), Time.fixedDeltaTime * _rotSpeed);

        //momentum pendientes, pa aregar momentum al player

        if (OnSlope())
        {
            _rb.velocity += Vector3.down * _slopeForce * Time.fixedDeltaTime;
        }
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
            if(AS.isPlaying && (AS.clip.name == "Hit_and_voice_1" || AS.clip.name == "Hit_and_voice_2" || AS.clip.name == "Hit_and_voice_3"))
            {
                int randomNum = Random.Range(1,2);
                AudioManager.instance.Play("Hit_"+randomNum);
            }
            else
            {
                PlayRandomSound(0, 3);
            }
        }
        else
        {
            LevelManager.Instance.currentExtraTimerBeforeKill = 0;
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

    bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            //Debug.Log($"Estoy inclinado un angulo de: {angle}");
            return angle >= _minSlopeAngleToBoostSpeed && angle != 0;
        }
        return false;
    }

    void StateAnimationCheck(AnxietyLevel state)
    {
        switch (state)
        {
            case AnxietyLevel.Zen:
                _anim.SetBool("Injured", false);
                break;
            case AnxietyLevel.Chill:
                _anim.SetBool("Injured", false);
                break;
            case AnxietyLevel.Alerted:
                _anim.SetBool("Injured", true);
                break;
            case AnxietyLevel.Traumatized:
                _anim.SetBool("Injured", true);
                break;
            case AnxietyLevel.Dead:
                _anim.SetBool("Injured", true);
                break;
        }
    }

    void PlaySound()
    {
        
    }

    void PlayRandomSound(int firstAudio, int lastAudio)
    {
        int soundNum = UnityEngine.Random.Range(11, 14);
        AS.clip = sounds[soundNum];
        AS.Play();
    }

    void OnGameOver()
    {
        Debug.Log("ongameover");
        _dead = true;
        _anim.SetTrigger("Dead");
    }
}
