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

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player"){
            GetComponent<MeshFilter>().mesh = ExplodedBubble;
        }
    }
}
