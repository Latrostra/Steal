using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindItem : MonoBehaviour
{
    public Transform closestItem;
    [SerializeField]
    private float _interactableDistance = 2f;
    private FieldOfView _fieldOfView;
    private float _distance;

    private void Awake() {
        _fieldOfView = GetComponent<FieldOfView>();
    }
    private void Update() {
        if (_fieldOfView.visibleTargets.Count <= 0) {
            closestItem = null;
            return;
        }
        _distance = float.MaxValue;
        foreach (var transform in _fieldOfView.visibleTargets) {
            if (transform == null) {
                continue;
            }
            if (Vector3.Distance(this.transform.position, transform.position) < _distance) {
                _distance = Vector3.Distance(this.transform.position, transform.position);
                closestItem = transform;
            }
        }
        if (closestItem == null) {
            return;
        }
        if (Vector3.Distance(this.transform.position, closestItem.position) > _interactableDistance) {
            _distance = float.MaxValue;
            closestItem = null;
        }
    }
}
