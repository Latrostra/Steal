using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PlayerInventory")]
public class PlayerInventorySO : ScriptableObject {
    public float MaxEndurance = 0;
    public float Endurance = 0;
    public int ItemWorth = 0;
}
