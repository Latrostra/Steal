using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Elementy UI przeniesc do ui managera!!!!!!!!!!!!!!!
public class Pick : MonoBehaviour, IPickable
{
    [SerializeField]
    private PlayerInventorySO _itemValue;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    [SerializeField]
    private float _pickUpDuration;
    [SerializeField]
    private Image _progressActionBar;
    [SerializeField]
    private Image _progressEnduranceBar;
    [SerializeField]
    private TextMeshProUGUI _text;
    [SerializeField]
    private StateSO state;
    private void Awake() {
        _playerInventory.ItemWorth = 0;
        _playerInventory.Endurance = 0;
        _progressEnduranceBar.fillAmount = Mathf.Lerp(0, 1, _playerInventory.Endurance / _playerInventory.MaxEndurance);
        _text.text = _playerInventory.ItemWorth + " $";
    }
    public void Use()
    {
        StartCoroutine("SetPickUpTimer");
    }

    private IEnumerator SetPickUpTimer() {
        float elapsedTime = 0f;
        if (_playerInventory.MaxEndurance - _playerInventory.Endurance < _itemValue.Endurance) {
            _progressActionBar.fillAmount = 1;
            _progressActionBar.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _progressActionBar.fillAmount = 0;
            _progressActionBar.color = Color.white;
            yield break;
        }
        state.IsBusy = true;
        while (elapsedTime < _pickUpDuration) {
            elapsedTime += Time.deltaTime;
            _progressActionBar.fillAmount = Mathf.Lerp(0, 1, elapsedTime / _pickUpDuration);
            yield return null;
        }

        _playerInventory.Endurance += _itemValue.Endurance;
        _playerInventory.ItemWorth += _itemValue.ItemWorth;
        _text.text = _playerInventory.ItemWorth + " $";
        _progressEnduranceBar.fillAmount = Mathf.Lerp(0, 1, _playerInventory.Endurance / _playerInventory.MaxEndurance);
        _progressActionBar.fillAmount = 0;
        state.IsBusy = false;

        Destroy(this.gameObject);
        yield return null;
    }
}
