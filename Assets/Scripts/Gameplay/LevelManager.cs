using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    float lvlTimer;
    float hamsterAnxietyNum;
    string hamsterAnxietyState;

    void Start()
    {
        
    }

    void Update()
    {
        lvlTimer -= Time.deltaTime;

        if(hamsterAnxietyNum < 70){
            hamsterAnxietyNum -= Time.deltaTime;
        }
        else{
            hamsterAnxietyNum -= Time.deltaTime/2;
        }

        switch(hamsterAnxietyNum){
            case float i when i > 0 && i <= 20:
                hamsterAnxietyState = "Zen";
            break;
            case float i when i > 20 && i <= 50:
                hamsterAnxietyState = "Chill";
            break;
            case float i when i > 50 && i <= 70:
                hamsterAnxietyState = "Alertado";
            break;
            case float i when i > 70 && i <= 99:
                hamsterAnxietyState = "Traumado";
            break;
            case 100:
                //Game Over
            break;
        }
    }
}
