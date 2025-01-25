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
                    stateMultiplier = stateMultiplierNormal;
                }
                break;
            case float i when i > 0 && i <= LvlTimer*alertedTier:
                if (hamsterAnxietyState != AnxietyLevel.Traumatized)
                {
                    ChangeState(AnxietyLevel.Traumatized);
                    stateMultiplier = stateMultiplierSlow;
                }
                break;
            case 0:
                GameOver();
            break;
        }
    }

    void ChangeState(AnxietyLevel level)
    {
        hamsterAnxietyState = level;
        hamsterStateFace.sprite = sprStates[(int)level];
        hamsterStateFace.gameObject.GetComponent<Animator>().Play(Animator.StringToHash("HamsterStateChange"));
    }

    void UIFillBar()
    {
        anxietyFill.fillAmount = 1 / (LvlTimer / AnxietyTimer);
    }

    public void GameOver()
    {

    }
}
