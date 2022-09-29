using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class WandererAi : MonoBehaviour, IEnemyAi
{
    [SerializeField]
    private GameObject _ground;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private float _awareDuration = 5f;
    private RandomPosition _randomPosition;
    private FieldOfView _fieldOfView;
    public bool IsRotating {get {
        return false;
    } 
    set {
        IsRotating = value;
    }}

    private void Awake() {
        _randomPosition = new RandomPosition(new List<Vector3>(_ground.GetComponent<MeshFilter>().sharedMesh.vertices), _ground);
        _fieldOfView = GetComponent<FieldOfView>();
    }

    private void Update() {
        if (_fieldOfView.visibleTargets.Count > 0) {
            _meshRenderer.material.SetFloat("_Step", 0);
            StartCoroutine("SetAware");
        }
    }

    public Vector3 CalculatePosition() {
        var pos = _randomPosition.CalculateRandomPoint();
        NavMesh.SamplePosition(pos, out var hit, Mathf.Infinity, 1);
        return hit.position;
    }
    
    private IEnumerator SetAware() {
        float elapsedTime = 0f;

        while (_fieldOfView.visibleTargets.Count > 0 && elapsedTime < _awareDuration) {
            elapsedTime += Time.deltaTime;
            _meshRenderer.material.SetFloat("_Step", Mathf.Lerp(6, -1, elapsedTime / _awareDuration));
            yield return null;
        }
        if (!(_fieldOfView.visibleTargets.Count > 0)) {
            _meshRenderer.material.SetFloat("_Step", 6);
        }
        if (elapsedTime >= _awareDuration) {
            SceneManager.LoadScene(0);
        }
        yield return null;
    }
}
