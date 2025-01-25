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
        AnxietyTimer = LvlTimer;
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
            case float i when i > LvlTimer*.6f && i <= LvlTimer*.8f:
                ChangeState(AnxietyLevel.Chill);
                break;
            case float i when i > LvlTimer*.4f && i <= LvlTimer*.6f:
                ChangeState(AnxietyLevel.Alerted);
                break;
            case float i when i > 0 && i <= LvlTimer*.4f:
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
