using UnityEngine;

public class DoorRequirement : MonoBehaviour
{
    public ItemClass itemdata; // detos de la llave requerida
    //
    public bool HasKeyToOpen()
    {
        InventoryManager inventoryManager = GetComponent<InventoryManager>();
        if (inventoryManager != null) 
        {
            return inventoryManager.IsOnInventory(itemdata);
        }
        return false;
    }
}
