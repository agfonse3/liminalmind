using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder; //guarda los slots
    [SerializeField] GameObject player;
    //public ItemClass itemToAdd; // item aadicionar al inventario para pruebas
    //private ItemClass itemToRemove;
    private List<ItemClass> items ; //lista de los items
    public Inventorylist playerInventorylist;
    private GameObject[] slots;
    public int quantityOfNotes;

    private void Start()
    {
        quantityOfNotes = 0;
        playerInventorylist = player.GetComponent<Playerdata>().Inventorylist;
        items = playerInventorylist.inventoryList;
        slots = new GameObject[slotsHolder.transform.childCount]; //cantidad de slots disponibles
        //colocar slots
        for (int i = 0; i < slotsHolder.transform.childCount;i++) 
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }
        UpdateInventoryUI();
        //AddItemsToInventory(itemToAdd); // para pruebas
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

    //metodo que adiciona items en el inventario
    public void AddItemsToInventory(ItemClass item) 
    {
        if (item != null) 
        {
            if (item.GetNote() != null)  //revisa si es una nota
            {
                if (quantityOfNotes == 0)
                {
                    items.Add(item); // si no hay la adiciona
                    quantityOfNotes++;
                }
                else if (quantityOfNotes<4)
                {
                    quantityOfNotes++;// si no aumenta la cantidad recolctadas
                }
            }else
            {
                if (item!=null)
                {
                    Debug.Log("adiciona un item");
                    Debug.Log(item);
                    items.Add(item);
                    UpdateInventoryUI();
                }
               
            }

            //Debug.Log("adiciona un item");
            //Debug.Log(item);
            //items.Add(item);
            //UpdateInventoryUI();
        }

    }

    //public void RemoveItemsFromInventory(ItemClass item)
    //{
    //    items.Remove(item);
    //}

    //metodo que verifica si hay un item en el inverntario
    public bool IsOnInventory(ItemClass item) 
    {
        for (int i = 0; i <items.Count; i++)
        {
            if (items != null && items[i] == item)
            {
                return true;
            }
        }
        return false;
    }


    // metodo para abrir la nota del panel
    public bool IsClickedNearNote(Vector3 mouseposition) 
    {
        int numberslotNote =-1;
        for (int y = 0; y < items.Count; y++)
        {
            if (items[y].GetNote()) 
            {
                numberslotNote = y;
                break;
            } 
        }
        if (numberslotNote!=-1) {
            if (Vector2.Distance(slots[numberslotNote].transform.position, mouseposition) < 15)
            {
                return true;
            }
        }
        return false;
                   
    }



}
