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
    [SerializeField] GameObject _brokenBallPrefab;
    [SerializeField] GameObject _deadExplosionParticlePrefab;
    [SerializeField] ParticleSystem _smokeParticles;

    Vector3 _checkpoint;
    Vector3 _lastValidDir;
    GameObject _camera;
    RaycastHit slopeHit;
    Animator _ballAnim;

    bool _dead;
    float _xAxis, _zAxis;

    //Sound
    [SerializeField] AudioClip[] sounds;
    AudioSource as_ball, as_hamster;

    bool isTalking;


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

        as_ball = GetComponent<AudioSource>();
        as_hamster = transform.GetChild(0).GetComponent<AudioSource>();
        _ballAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_rb.velocity.y > 8) _ballAnim.SetTrigger("Jump");
        if (_rb.velocity.y < -5) _ballAnim.SetTrigger("Falling");

        if (_rb.velocity.magnitude > 18f && !_smokeParticles.isPlaying)
        {
            PlaySmokeParticles();
            AudioManager.instance.Play("Whoosh_Speed_1");
            if (!isTalking)
            {
                as_hamster.clip = sounds[5];
                as_hamster.Play();
                isTalking = true;
            }
        }
        _xAxis = Input.GetAxis("Horizontal");
        _zAxis = Input.GetAxis("Vertical");
        if (transform.position.y <= _falYThreshold) PlayerFellOutOfMap();

        if(isTalking && !as_hamster.isPlaying)
        {
            isTalking = false;
        }

//#if UNITY_EDITOR
        //Cheatcodes, solo funcionan en el editor
        if (Input.GetKeyDown(KeyCode.Return)) UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        if (Input.GetKeyDown(KeyCode.Space)) _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        if (Input.GetKeyDown(KeyCode.F)) TakeDamage(100f);
        //Con Enter restarteo la escena, para evitar estar poniendo y sacando play todo el tiempo
//#endif
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
            if(as_hamster.isPlaying && isTalking)
            {
                //Si se está reproduciendo un sonido con su voz
                int randomNum = Random.Range(1,3);
                AudioManager.instance.Play("Hit_"+randomNum);
            }
            else
            {
                PlayRandomSound(true, 0, 3);
                isTalking = true;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!as_ball.isPlaying)
        {
            PlayRandomSoundByName(false, "Hit_Floor_Caida_", 1, 3);
            if(_rb.velocity.magnitude > 5) _ballAnim.SetTrigger("Crash");
            //Debug.Log($"Crashié con magnitud {_rb.velocity.magnitude}");
        }
    }

    void PlaySound()
    {
        
    }

    public void PlaySmokeParticles()
    {
        _smokeParticles.Play();
    }

    /// <summary>
    /// rdsrtsdfgttsdgt
    /// </summary>
    /// <param name="hamster">Si modifca el Hamster (true) o la pelota</param>
    /// <param name="firstAudio"></param>
    /// <param name="lastAudio"></param>
    void PlayRandomSound(bool hamster, int firstAudio, int lastAudio)
    {
        int soundNum = UnityEngine.Random.Range(firstAudio, lastAudio);
        if (hamster)
        {
            as_hamster.clip = sounds[soundNum];
            as_hamster.Play();
        }
        else
        {
            as_ball.clip = sounds[soundNum];
            as_ball.Play();
        }
    }

    void PlayRandomSoundByName(bool hamster, string str, int firstIndex, int lastIndex)
    {
        int randomNum = Random.Range(firstIndex, lastIndex);
        foreach(AudioClip clip in sounds)
        {
            if(clip.name == str + randomNum)
            {
                as_ball.clip = clip;
            }
        }
        
        as_ball.Play();
    }

    public void WinSequence()
    {
        LevelManager.Instance.PauseGame();
        Debug.Log("ongameover");
        _rb.velocity = Vector3.zero;
        _dead = true;
        _anim.SetTrigger("Victory");
        Camera.main.GetComponent<CameraController>().CloseLookup = true;
        StartCoroutine(GameWonSequence());
        AudioManager.instance.Play("Feliz_2");
        LevelManager.Instance.WinConfetti();
    }

    IEnumerator GameWonSequence()
    {
        yield return new WaitForSeconds(4.5f);
        LevelManager.Instance.WinScreenAppear();
    }

    void OnGameOver()
    {
        Debug.Log("ongameover");
        _rb.velocity = Vector3.zero;
        GetComponent<Renderer>().enabled = false;
        Instantiate(_brokenBallPrefab, transform.position, Quaternion.identity);
        Instantiate(_deadExplosionParticlePrefab, transform.position, Quaternion.identity);
        _dead = true;
        _anim.SetTrigger("Dead");
        Camera.main.GetComponent<CameraController>().CloseLookup = true;
        StartCoroutine(GameOverSequence());
        AudioManager.instance.Play("Muerto_" + Random.Range(1,3));
    }

    IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(3f);
        LevelManager.Instance.DeadScreenAppear();
    }
}
