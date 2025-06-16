using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryListScriptableObject", menuName = "InventoryList")]
public class Inventorylist : ScriptableObject
{
    public List<ItemClass> inventoryList = new List<ItemClass>();

    public void ResetData()
    {
        inventoryList.Clear();
    }

}
