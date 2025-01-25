using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    AnxietyLevel hamsterAnxietyState;

    [SerializeField]
    float lvlTimer; //Valor inicial

    public float AnxietyTimer { get; set; } //Ansiedad actual

    //Bubbles
    public int totalBubbles;
    int bubblesLeft;

    //State
    [SerializeField] Image hamsterStateFace;
    [SerializeField] Sprite[] sprStates;

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        AnxietyTimer = lvlTimer;
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

        switch(AnxietyTimer){
            case float i when i > lvlTimer*.8f && i <= lvlTimer:
                ChangeState(AnxietyLevel.Zen);
            break;
            case float i when i > lvlTimer*.6f && i <= lvlTimer*.8f:
                ChangeState(AnxietyLevel.Chill);
                break;
            case float i when i > lvlTimer*.4f && i <= lvlTimer*.6f:
                ChangeState(AnxietyLevel.Alerted);
                break;
            case float i when i > 0 && i <= lvlTimer*.4f:
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

    public void GameOver()
    {

    }
}
