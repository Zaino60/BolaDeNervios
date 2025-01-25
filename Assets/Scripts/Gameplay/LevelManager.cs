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

    [SerializeField] float zenTier = .8f, chillTier = .6f, alertedTier = .4f;

    //Bubbles
    public int totalBubbles;
    int bubblesLeft;

    //State
    [SerializeField] Image hamsterStateFace;
    [SerializeField] Sprite[] sprStates;
    [SerializeField] Image anxietyFill;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        AnxietyTimer = LvlTimer*zenTier;
        bubblesLeft = totalBubbles;
    }

    void Update()
    {
        //
        if(AnxietyTimer > 20){
            AnxietyTimer -= Time.deltaTime;
        }
        else{
            AnxietyTimer -= Time.deltaTime/2;
        }

        UIFillBar();

        switch (AnxietyTimer){
            case float i when i > LvlTimer*.8f && i <= LvlTimer:
                ChangeState(AnxietyLevel.Zen);
            break;
            case float i when i > LvlTimer*chillTier && i <= LvlTimer*zenTier:
                ChangeState(AnxietyLevel.Chill);
                break;
            case float i when i > LvlTimer*alertedTier && i <= LvlTimer* chillTier:
                ChangeState(AnxietyLevel.Alerted);
                break;
            case float i when i > 0 && i <= LvlTimer*alertedTier:
                ChangeState(AnxietyLevel.Traumatized);
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
    }

    void UIFillBar()
    {
        anxietyFill.fillAmount = 1 / (LvlTimer / AnxietyTimer);
    }

    public void GameOver()
    {

    }
}
