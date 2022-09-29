using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AwareController : MonoBehaviour
{
    [SerializeField]
    private float _awareDuration = 5f;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    private FieldOfView _fieldOfView;

    private void Awake() {
        _fieldOfView = GetComponent<FieldOfView>();
    }

    private void Update() {
        if (_fieldOfView.visibleTargets.Count > 0) {
            _meshRenderer.material.SetFloat("_Step", 0);
            StartCoroutine("SetAware");
        }
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
