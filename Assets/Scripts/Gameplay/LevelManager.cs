using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    AnxietyLevel hamsterAnxietyState;

    [SerializeField]
    float lvlTimer; //Valor inicial

    public float AnxietyTimer { get; set; } //Ansiedad actual

    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    void Start()
    {
        AnxietyTimer = lvlTimer;
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
                hamsterAnxietyState = AnxietyLevel.Zen;
            break;
            case float i when i > lvlTimer*.6f && i <= lvlTimer*.8f:
                hamsterAnxietyState = AnxietyLevel.Chill;
            break;
            case float i when i > lvlTimer*.4f && i <= lvlTimer*.6f:
                hamsterAnxietyState = AnxietyLevel.Alerted;
            break;
            case float i when i > 0 && i <= lvlTimer*.4f:
                hamsterAnxietyState = AnxietyLevel.Traumatized;
            break;
            case 0:
                //Game Over
            break;
        }
    }
}
public enum AnxietyLevel{Zen, Chill, Alerted, Traumatized}
