using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private PlayerAction _input;
    private Rigidbody _rigibody;
    private void Awake() {
        _input = GetComponent<PlayerAction>();
        _rigibody = GetComponent<Rigidbody>();
    }

    private void Update() {
        RotateToMouse();
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        if (_input.moveVector == Vector2.zero) {
            return;
        }
        _rigibody.MovePosition(_rigibody.position + new Vector3(_input.moveVector.x, 0, _input.moveVector.y) * speed * Time.fixedDeltaTime);
    }

    private void RotateToMouse() {
        transform.LookAt(_input.mouseVector + Vector3.up * transform.position.y);
    }
}
