using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private PlayerAction _input;
    private Rigidbody _rigidbody;
    private FindItem _findItem;
    [SerializeField]
    private StateSO state;

    private void Awake() {
        _input = GetComponent<PlayerAction>();
        _rigidbody = GetComponent<Rigidbody>();
        _findItem = GetComponent<FindItem>();

        state.IsBusy = false;

        _input.onInteraction += InteractHandler;
    }

    private void OnDestroy() {
        _input.onInteraction -= InteractHandler;
    }

    private void Update() {
        RotateToMouse();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (_input.moveVector == Vector2.zero || state.IsBusy) {
            return;
        }
        Vector3 direction = (_rigidbody.position + new Vector3(_input.moveVector.x, 0, _input.moveVector.y) - _rigidbody.position).normalized;
        Ray ray = new Ray(_rigidbody.position, direction);
        RaycastHit hit;
        if (!Physics.Raycast(ray,out hit,direction.magnitude - 1f)) {
            _rigidbody.MovePosition(_rigidbody.position + new Vector3(_input.moveVector.x, 0, _input.moveVector.y) * speed * Time.fixedDeltaTime);
        }
        else {
            _rigidbody.MovePosition(hit.point);
        }
    }

    private void RotateToMouse() {
        if (state.IsBusy) {
            return;
        }
        transform.LookAt(_input.mouseVector + Vector3.up * transform.position.y);
    }

    private void InteractHandler() {
        if (state.IsBusy || _findItem.closestItem == null) {
            return;
        }
        _findItem.closestItem.gameObject.GetComponent<IPickable>().Use();
    }
}
