using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder; //guarda los slots
    [SerializeField] private ItemClass itemToAdd; // item aadicionar al inventario
    //private ItemClass itemToRemove;
    public List<ItemClass> items = new List<ItemClass>(); //lista de los items
    private GameObject[] slots;
    private int quantityOfNotes;

    private void Start()
    {
        quantityOfNotes = 0;
        slots = new GameObject[slotsHolder.transform.childCount]; //cantidad de slots disponibles
        //colocar slots
        for (int i = 0; i < slotsHolder.transform.childCount;i++) 
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }
        UpdateInventoryUI();
        AddItemsToInventory(itemToAdd);
    }
    // metodo que actualiza el inventario
    public void UpdateInventoryUI() 
    {
        Debug.Log("entra al update");
        for (int i = 0; i < slots.Length ; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemsprite;

            }
            catch 
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
            
        }
    }


    public void AddItemsToInventory(ItemClass item) 
    {
        items.Add(item);
        UpdateInventoryUI();
    }

    //public void RemoveItemsFromInventory(ItemClass item)
    //{
    //    items.Remove(item);
    //}


}
