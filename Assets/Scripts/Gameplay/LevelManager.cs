using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public AnxietyLevel HamsterAnxietyState { get; set; }

    [field:SerializeField] public float LvlTimer { get; set; } //Valor inicial

    public float AnxietyTimer { get; set; } //Ansiedad actual

    [SerializeField] float zenTier = .7f, chillTier = .4f, alertedTier = .1f;

    //Camera
    [SerializeField] CinemachineFreeLook mainCam;

    //Bubbles
    public int totalBubbles { get; set; }
    public int bubblesLeft { get; set; }

    //State
    [SerializeField] Image hamsterStateFace;
    [SerializeField] Sprite[] sprStates;
    [SerializeField] Image anxietyFill;
    [SerializeField] GameObject _deadScreen;
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _winConfettiPrefab;
    [SerializeField] GameObject _winConfettiSpawnPoint;
    float stateMultiplier, stateMultiplierNormal = 1, stateMultiplierSlow = .75f;
    float extraTimerBeforeKill = 2;
    public float currentExtraTimerBeforeKill { get; set; }
    Animator hamsterStateAnim;

    public Action<AnxietyLevel> OnStateChange = delegate { };
    public Action OnGameOver = delegate { };
    public Action OnGameWon = delegate { };

    //Background Music
    float zenPitch = 1.1f, chillPitch = 1, alertedPitch = 0.8f, traumatizedPitch = 0.6f;
    AudioSource musicAS;
    bool _paused;


    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        Cursor.visible = false;
        AnxietyTimer = LvlTimer*zenTier;
        stateMultiplier = stateMultiplierNormal;
        hamsterStateAnim = hamsterStateFace.gameObject.GetComponent<Animator>();
        musicAS = GetComponent<AudioSource>();

        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        bubblesLeft = totalBubbles;
    }

    void Update()
    {
        UIFillBar();

        if(!_paused) AnxietyTimer -= Time.deltaTime * stateMultiplier;

        switch (AnxietyTimer){
            case float i when i > LvlTimer*.8f && i <= LvlTimer:
                if(HamsterAnxietyState != AnxietyLevel.Zen)
                {
                    ChangeState(AnxietyLevel.Zen);
                    AudioManager.instance.Play("Feliz_2");
                    musicAS.pitch = zenPitch;
                }
            break;
            case float i when i > LvlTimer*chillTier && i <= LvlTimer*zenTier:
                if (HamsterAnxietyState != AnxietyLevel.Chill)
                {
                    ChangeState(AnxietyLevel.Chill);
                    AudioManager.instance.Play("Feliz_1");
                    musicAS.pitch = chillPitch;
                }
                break;
            case float i when i > LvlTimer*alertedTier && i <= LvlTimer* chillTier:
                if (HamsterAnxietyState != AnxietyLevel.Alerted)
                {
                    ChangeState(AnxietyLevel.Alerted);
                    AudioManager.instance.Play("Traumado_2");
                    musicAS.pitch = alertedPitch;
                }
                break;
            case float i when i > 0 && i <= LvlTimer*alertedTier:
                if (HamsterAnxietyState != AnxietyLevel.Traumatized)
                {
                    ChangeState(AnxietyLevel.Traumatized);
                    AudioManager.instance.Play("Traumado_1");
                    musicAS.pitch = traumatizedPitch;
                }
                break;
            case float i when i <= 0 && HamsterAnxietyState != AnxietyLevel.Dead:
                LvlTimer = 0;
                currentExtraTimerBeforeKill -= Time.deltaTime;
                print(currentExtraTimerBeforeKill);
                if (currentExtraTimerBeforeKill <= 0)
                {
                    GameOver();

                }
            break;
        }
    }

    void ChangeState(AnxietyLevel level)
    {
        if (HamsterAnxietyState == AnxietyLevel.Traumatized && level == AnxietyLevel.Alerted)
        {
            stateMultiplier = stateMultiplierNormal;
            hamsterStateFace.gameObject.GetComponent<Animator>().speed = 1;
        }
        else if (HamsterAnxietyState == AnxietyLevel.Alerted && level == AnxietyLevel.Traumatized)
        {
            stateMultiplier = stateMultiplierSlow;
            hamsterStateAnim.speed = 1.5f;
            currentExtraTimerBeforeKill = extraTimerBeforeKill;
        }

        OnStateChange(level);
        HamsterAnxietyState = level;
        ChangeStateFace((int)level);
        hamsterStateAnim.Play(Animator.StringToHash("HamsterStateChange"));
    }

    void UIFillBar()
    {
        anxietyFill.fillAmount = 1 / (LvlTimer / AnxietyTimer);
    }

    public void GameOver()
    {
        print("Game Over!");
        OnGameOver();
        ChangeState(AnxietyLevel.Dead);
        hamsterStateAnim.speed = 0;
    }

    public void PauseGame()
    {
        _paused = true;
    }

    public void ChangeStateFace(int face)
    {
        hamsterStateFace.sprite = sprStates[face];
    }
    public void ActivatePleasure()
    {
        StartCoroutine(Pleasure());
    }

    IEnumerator Pleasure()
    {
        if (!hamsterStateAnim.GetCurrentAnimatorStateInfo(0).IsName("HamsterPleasure"))
        {
            hamsterStateAnim.Play(Animator.StringToHash("HamsterPleasure"));
            int actualFace = (int)HamsterAnxietyState;
            ChangeStateFace(5);
            yield return new WaitForSeconds(1);
            ChangeStateFace(actualFace);
        }
    }

    public void WinScreenAppear()
    {
        Cursor.visible = true;
        _winScreen.SetActive(true);

        _winScreen.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "¡Sobreviste! \n Y explotaste " + (totalBubbles - bubblesLeft) + "/" + totalBubbles + " burbujas.";
    }

    public void WinConfetti()
    {
        Instantiate(_winConfettiPrefab, _winConfettiSpawnPoint.transform.position, Quaternion.identity, _winConfettiSpawnPoint.transform);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeadScreenAppear()
    {
        Cursor.visible = true;
        _deadScreen.SetActive(true);
    }

    public void CameraShake(float intensity, float time)
    {
        mainCam.GetComponent<CamShakeController>().CameraShake(intensity, time);
    }

    public Transform HamsterFace => hamsterStateFace.transform;
}
