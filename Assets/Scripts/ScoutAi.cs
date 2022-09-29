using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ScoutAi : MonoBehaviour, IEnemyAi
{
    public bool IsRotating {get; set;}
    [SerializeField]
    private List<GameObject> _waypoint = new List<GameObject>();
    [SerializeField]
    private int _index = 0;
    private Vector3 _lastFacing;
    private void Awake() {
        _lastFacing = transform.eulerAngles;
    }
    private void Update() {
        Vector3 currentFacing = transform.forward;
        var currentAngularVelocity = Vector3.Angle(currentFacing, _lastFacing) / Time.deltaTime;
        _lastFacing = currentFacing;

        IsRotating = currentAngularVelocity > 0.05f;
    }
    public Vector3 CalculatePosition()
    {
        _index++;
        if (_waypoint.Count - 1 < _index) {
            _index = 0;
        }
        return _waypoint[_index].transform.position;
    }
}
