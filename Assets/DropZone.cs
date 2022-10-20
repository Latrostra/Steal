using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    [SerializeField]
    private float _dropDuration;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    [SerializeField]
    private GameResultSO _gameResult;
    private IEnumerator _func;

    void OnTriggerEnter(Collider col) {
        _func = SetDropTimer();
        if (_playerInventory.ItemWorth <= 0) {
            return;
        }
        StartCoroutine(_func);
    }

    void OnTriggerExit(Collider col) {
        StopCoroutine(_func);
        UiManager.Instance.SetProgressBarNormalState();
    }

    private IEnumerator SetDropTimer() {
        float elapsedTime = 0f;
        while (elapsedTime < _dropDuration) {
            elapsedTime += Time.deltaTime;
            UiManager.Instance.SetProgressBarState(elapsedTime, _dropDuration);
            yield return null;
        }
        _gameResult.Score += _playerInventory.ItemWorth;
        UiManager.Instance.SetScoreText(_gameResult.Score.ToString());
        _playerInventory.ItemWorth = 0;
        _playerInventory.Endurance = 0;
        UiManager.Instance.SetProgressBarNormalState();
        UiManager.Instance.SetItemWorthText(_playerInventory.ItemWorth + " $");
        UiManager.Instance.SetEnduranceBarState(_playerInventory.Endurance, _playerInventory.MaxEndurance);
        yield return null;
    }
}
