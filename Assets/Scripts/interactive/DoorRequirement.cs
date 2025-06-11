using UnityEngine;

public class DoorRequirement : MonoBehaviour
{
    public ItemClass itemdata; // detos de la llave requerida
    private InventoryManager inventoryManager;
    public GameObject inventory;
    //

    private void Start()
    {
        inventoryManager = inventory.GetComponent<InventoryManager>();
    }
    public bool HasKeyToOpen()
    {
        if (inventoryManager != null) 
        {
            return inventoryManager.IsOnInventory(itemdata);
        }
        return false;
    }
}
