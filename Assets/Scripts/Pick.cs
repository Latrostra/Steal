using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jak kiedyś Ci się zachce zrób z tego jakis event czy cos moronie
public class Pick : MonoBehaviour, IPickable
{
    [SerializeField]
    private PlayerInventorySO _itemValue;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    [SerializeField]
    private float _pickUpDuration;
    [SerializeField]
    private StateSO state;

    public void Use()
    {
        StartCoroutine("SetPickUpTimer");
    }

    private IEnumerator SetPickUpTimer() {
        float elapsedTime = 0f;
        if (_playerInventory.MaxEndurance - _playerInventory.Endurance < _itemValue.Endurance) {
            UiManager.Instance.SetProgressBarAlertState();
            yield return new WaitForSeconds(0.5f);
            UiManager.Instance.SetProgressBarNormalState();
            yield break;
        }
        state.IsBusy = true;
        while (elapsedTime < _pickUpDuration) {
            elapsedTime += Time.deltaTime;
            UiManager.Instance.SetProgressBarState(elapsedTime, _pickUpDuration);
            yield return null;
        }

        _playerInventory.Endurance += _itemValue.Endurance;
        _playerInventory.ItemWorth += _itemValue.ItemWorth;
        UiManager.Instance.SetItemWorthText(_playerInventory.ItemWorth + " $");
        UiManager.Instance.SetEnduranceBarState(_playerInventory.Endurance, _playerInventory.MaxEndurance);
        UiManager.Instance.progressActionBar.fillAmount = 0;
        state.IsBusy = false;

        Destroy(this.gameObject);
        yield return null;
    }
}
