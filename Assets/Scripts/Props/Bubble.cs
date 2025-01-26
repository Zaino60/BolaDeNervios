using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]int healNum = 2;

    [SerializeField] Mesh ExplodedBubble;
    [SerializeField] Material[] ExplodedBubbleMaterials;

    [SerializeField] GameObject _popParticlePrefab;

    void Start()
    {
        LevelManager.Instance.totalBubbles++;
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.GetComponent<Hamster>())
        {
            GetComponent<MeshFilter>().mesh = ExplodedBubble;
            GetComponent<MeshRenderer>().materials = ExplodedBubbleMaterials;

            AudioManager.instance.PlayRandomSound(0, 6);

            coll.GetComponent<Hamster>().Heal(healNum);
            LevelManager.Instance.bubblesLeft--;

            LevelManager.Instance.ActivatePleasure();
            ParticleImage particle = Instantiate(_popParticlePrefab, Camera.main.WorldToScreenPoint(transform.position), Quaternion.identity, LevelManager.Instance.HamsterFace).GetComponent<ParticleImage>();
            particle.attractorTarget = LevelManager.Instance.HamsterFace;

            Destroy(this);
        }
    }
}
