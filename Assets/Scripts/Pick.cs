using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pick : MonoBehaviour, IPickable
{
    [SerializeField]
    private PlayerInventorySO _itemValue;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    [SerializeField]
    private float _pickUpDuration;
    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private StateSO state;

    public void Use()
    {
        StartCoroutine("SetPickUpTimer");
    }

    private IEnumerator SetPickUpTimer() {
        float elapsedTime = 0f;
        state.IsBusy = true;
        while (elapsedTime < _pickUpDuration) {
            elapsedTime += Time.deltaTime;
            progressBar.fillAmount = Mathf.Lerp(0, 1, elapsedTime / _pickUpDuration);
            yield return null;
        }

        _playerInventory.Endurance += _itemValue.Endurance;
        _playerInventory.ItemWorth += _itemValue.ItemWorth;

        progressBar.fillAmount = 0;
        state.IsBusy = false;

        Destroy(this.gameObject);
        yield return null;
    }
}
