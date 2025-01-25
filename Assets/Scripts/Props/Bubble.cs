using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    Mesh ExplodedBubble;
    
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player"){
            GetComponent<MeshFilter>().mesh = ExplodedBubble;
            //AudioManager.Instance.Play("BubblePop");
        }
    }
}
