using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]int healNum = 2;

    [SerializeField] Mesh ExplodedBubble;
    [SerializeField] Material[] ExplodedBubbleMaterials;

    void Awake()
    {
        LevelManager.Instance.totalBubbles++;
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.GetComponent<Hamster>())
        {
            GetComponent<MeshFilter>().mesh = ExplodedBubble;
            GetComponent<MeshRenderer>().materials = ExplodedBubbleMaterials;
            AudioManager.instance.Play("BubblePop");
            
            coll.GetComponent<Hamster>().Heal(healNum);

            Destroy(this);
        }
    }
}
