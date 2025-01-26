using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelWinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hamster>())
        {
            other.GetComponent<Hamster>().WinSequence();
        }
    }
}
