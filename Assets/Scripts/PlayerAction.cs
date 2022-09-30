using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerAction : MonoBehaviour
{
    public Vector2 moveVector;
    public Vector3 mouseVector;
    public Action onInteraction;
    private void OnMove(InputValue value) {
        moveVector = value.Get<Vector2>();
    }
    private void OnMouseMove(InputValue value) {
        var pos = value.Get<Vector2>();
        mouseVector = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, Camera.main.transform.position.y));
    }
    private void OnInteract(InputValue value) {
        onInteraction?.Invoke();
    }
}
