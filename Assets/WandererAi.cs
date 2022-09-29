using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WandererAi : MonoBehaviour, IEnemyAi
{
    [SerializeField]
    private GameObject _ground;
    private RandomPosition _randomPosition;
    
    public bool IsRotating {get {
        return false;
    }}

    private void Awake() {
        _randomPosition = new RandomPosition(new List<Vector3>(_ground.GetComponent<MeshFilter>().sharedMesh.vertices), _ground);
    }

    public Vector3 CalculatePosition() {
        var pos = _randomPosition.CalculateRandomPoint();
        NavMesh.SamplePosition(pos, out var hit, Mathf.Infinity, 1);
        return hit.position;
    }
}
