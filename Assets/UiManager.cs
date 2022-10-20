using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    public Image progressActionBar;
    public Image progressEnduranceBar;
    public TextMeshProUGUI enduranceText;
    public TextMeshProUGUI scoreText;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    [SerializeField]
    private GameResultSO _gameResult;
    private void Awake() {
        Instance = this;
        _playerInventory.ItemWorth = 0;
        _playerInventory.Endurance = 0;
        _gameResult.Score = 0;
        SetScoreText(_gameResult.Score.ToString());
        SetEnduranceBarState(_playerInventory.Endurance, _playerInventory.MaxEndurance);
        SetItemWorthText(_playerInventory.ItemWorth + " $");
    }

    public void SetProgressBarAlertState() {
        progressActionBar.fillAmount = 1;
        progressActionBar.color = Color.red;
    }

    public void SetProgressBarNormalState() {
        progressActionBar.fillAmount = 0;
        progressActionBar.color = Color.white;
    }

    public void SetProgressBarState(float elapsedTime, float duration) {
        progressActionBar.fillAmount = Mathf.Lerp(0, 1, elapsedTime / duration);
    }

    public void SetEnduranceBarState(float endurance, float maxEndurance) {
        progressEnduranceBar.fillAmount = Mathf.Lerp(0, 1, endurance / maxEndurance);
    }

    public void SetItemWorthText(string text) {
        enduranceText.text = text;
    }
    public void SetScoreText(string text) {
        scoreText.text = text;
    }

    public void SetTimer() {
        
    }
}
