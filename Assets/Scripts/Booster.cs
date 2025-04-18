using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _boostForce;
    [Header("Camera Shake")]
    float _camShakeIntensity;
    [SerializeField] float _camShakeTime = 0.1f;
    Renderer _mat;
    float _offsetTime;
    
    Animator anim;

    private void Start()
    {
        _mat = GetComponentInChildren<Renderer>();
        anim = GetComponent<Animator>();
        _camShakeIntensity = _boostForce/1000;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hamster>())
        {
            Debug.Log("boost");
            //other.GetComponent<Hamster>().PlaySmokeParticles();
            other.GetComponent<Rigidbody>().AddForce(transform.forward * _boostForce);
            AudioManager.instance.Play("Whoosh_Speed_3",.6f,Random.Range(1, 1.1f));
            if(anim) anim.Play(Animator.StringToHash("BoosterActivated"));
            //StartCoroutine(BoostEffect());

            //Camera Shake
            if (LevelManager.Instance)
            {
                LevelManager.Instance.CameraShake(_camShakeIntensity, _camShakeTime);
            }
        }
    }

    //IEnumerator BoostEffect()
    //{
    //    _offsetTime = 0;
    //    for (int i = 0; i < 90; i++)
    //    {
    //        _offsetTime += Time.deltaTime * 10f;
    //        yield return null;
    //        _mat.material.SetTextureOffset("_BaseMap", new Vector2(0f, _offsetTime));
    //    }
    //    _mat.material.SetTextureOffset("_BaseMap", new Vector2(0f, 0f));
    //}
}
