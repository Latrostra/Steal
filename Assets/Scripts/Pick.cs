using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour, IPickable
{
    [SerializeField]
    private PlayerInventorySO _itemValue;
    [SerializeField]
    private PlayerInventorySO _playerInventory;
    public void Use()
    {
        _playerInventory.Endurance += _itemValue.Endurance;
        _playerInventory.ItemWorth += _itemValue.ItemWorth;
        Destroy(this.gameObject);
    }
}
