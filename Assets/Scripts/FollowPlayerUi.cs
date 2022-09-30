using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerUi : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;
    private void Update() {
        this.transform.position = _object.transform.position;
    }
}
