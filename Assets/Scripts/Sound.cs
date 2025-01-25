using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public AudioSource audioS;

    private void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }
    private void Start()
    {
        Destroy(gameObject, audioS.clip.length);
        audioS.Play();
    }
}
