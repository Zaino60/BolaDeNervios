using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    AnxietyLevel hamsterAnxietyState;

    [field:SerializeField] public float LvlTimer { get; set; } //Valor inicial

    public float AnxietyTimer { get; set; } //Ansiedad actual

    [SerializeField] float zenTier = .7f, chillTier = .4f, alertedTier = .1f;

    //Bubbles
    public int totalBubbles;
    int bubblesLeft;

    //State
    [SerializeField] Image hamsterStateFace;
    [SerializeField] Sprite[] sprStates;
    [SerializeField] Image anxietyFill;
    float stateMultiplier, stateMultiplierNormal = 1, stateMultiplierSlow = .75f;
    float extraTimerBeforeKill = 2;
    public float currentExtraTimerBeforeKill { get; set; };
    Animator hamsterStateAnim;


    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        AnxietyTimer = LvlTimer*zenTier;
        stateMultiplier = stateMultiplierNormal;
        bubblesLeft = totalBubbles;
        hamsterStateAnim = hamsterStateFace.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        UIFillBar();

        AnxietyTimer -= Time.deltaTime * stateMultiplier;

        switch (AnxietyTimer){
            case float i when i > LvlTimer*.8f && i <= LvlTimer:
                if(hamsterAnxietyState != AnxietyLevel.Zen)
                {
                    ChangeState(AnxietyLevel.Zen);
                }
            break;
            case float i when i > LvlTimer*chillTier && i <= LvlTimer*zenTier:
                if (hamsterAnxietyState != AnxietyLevel.Chill)
                {
                    ChangeState(AnxietyLevel.Chill);
                }
                break;
            case float i when i > LvlTimer*alertedTier && i <= LvlTimer* chillTier:
                if (hamsterAnxietyState != AnxietyLevel.Alerted)
                {
                    ChangeState(AnxietyLevel.Alerted);
                }
                break;
            case float i when i > 0 && i <= LvlTimer*alertedTier:
                if (hamsterAnxietyState != AnxietyLevel.Traumatized)
                {
                    ChangeState(AnxietyLevel.Traumatized);
                }
                break;
            case float i when i <= 0 && hamsterAnxietyState != AnxietyLevel.Dead:
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
        if (hamsterAnxietyState == AnxietyLevel.Traumatized && level == AnxietyLevel.Alerted)
        {
            stateMultiplier = stateMultiplierNormal;
            hamsterStateFace.gameObject.GetComponent<Animator>().speed = 1;
        }
        else if (hamsterAnxietyState == AnxietyLevel.Alerted && level == AnxietyLevel.Traumatized)
        {
            stateMultiplier = stateMultiplierSlow;
            hamsterStateAnim.speed = 1.5f;
            currentExtraTimerBeforeKill = extraTimerBeforeKill;
        }

        hamsterAnxietyState = level;
        hamsterStateFace.sprite = sprStates[(int)level];
        hamsterStateAnim.Play(Animator.StringToHash("HamsterStateChange"));
    }

    void UIFillBar()
    {
        anxietyFill.fillAmount = 1 / (LvlTimer / AnxietyTimer);
    }

    public void GameOver()
    {
        print("Game Over!");
        ChangeState(AnxietyLevel.Dead);
        hamsterStateAnim.speed = 0;
    }
}
