using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private FindItem _findItem;
    private MeshRenderer _meshRenderer;

    private void Awake() {
        _findItem = FindObjectOfType<FindItem>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update() {
        if (_findItem.closestItem != null && _findItem.closestItem.gameObject == this.gameObject) {
            _meshRenderer.material.SetFloat("_Glossiness", 1f);
            _meshRenderer.material.SetFloat("_Metallic", 1f);
        }
        else if (_meshRenderer.material.GetFloat("_Metallic") == 1f || _meshRenderer.material.GetFloat("_Glossiness") == 1f) {
            _meshRenderer.material.SetFloat("_Glossiness", 0f);
            _meshRenderer.material.SetFloat("_Metallic", 0f);
        }
    }
}
