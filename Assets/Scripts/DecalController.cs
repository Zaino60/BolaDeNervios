using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalController : MonoBehaviour
{
    [SerializeField] DecalProjector decal; // Assign the Decal Projector
    [SerializeField] float maxProjectionDistance = 10f; // Maximum decal range
    [SerializeField] float minProjectionDistance = .5f; // Min decal range
    [SerializeField] float _error = .2f;
    [SerializeField] float _defaultOpacity = .8f;
    float _alpha;
    [SerializeField] float _alphaRedSpeed = 4f;

    //public LayerMask layerMask; // Layers the decal should project onto

    void Update()
    {
        AdjustDecal();
    }

    void AdjustDecal()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxProjectionDistance))
        {
            if (hit.distance <= minProjectionDistance)
            {
                _alpha = Mathf.Clamp(_alpha - (Time.deltaTime * _alphaRedSpeed), 0f, _defaultOpacity);
                decal.fadeFactor = _alpha;
                return;
            }
            else
            {
                _alpha = Mathf.Clamp(_alpha + (Time.deltaTime * _alphaRedSpeed), 0f, _defaultOpacity);
                decal.fadeFactor = _alpha;
                decal.enabled = true;
                decal.size = new Vector3(decal.size.x, decal.size.y, hit.distance + _error);
                decal.pivot = new Vector3(0f, 0f, hit.distance / 2f);
            }
        }
    }
}
