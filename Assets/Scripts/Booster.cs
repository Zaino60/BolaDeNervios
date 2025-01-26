using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] float _boostForce;
    Renderer _mat;
    float _offsetTime;
    
    Animator anim;

    private void Start()
    {
        _mat = GetComponentInChildren<Renderer>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Hamster>())
        {
            Debug.Log("boost");
            other.GetComponent<Rigidbody>().AddForce(transform.forward * _boostForce);
            AudioManager.instance.Play("Whoosh_Speed_3",1f,Random.Range(1, 1.1f));
            anim.Play(Animator.StringToHash("BoosterActivated"));
            //StartCoroutine(BoostEffect());
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
