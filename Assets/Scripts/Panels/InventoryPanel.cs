using System.Collections;
using UnityEngine;

public class InventoryPanel : PanelBasic
{
    [SerializeField] GameObject InventoryPannel; //panel activo
    private InventoryManager inventoryManager;
    [SerializeField] private GameObject inventory;
    private bool isOnNote = false;

    private void Start()
    {
        inventoryManager = inventory.GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //si click
        {
            isOnNote=inventoryManager.IsClickedNearNote(Input.mousePosition);
            if (isOnNote) 
            {
                ShowNote();
                StartCoroutine(DontShowNote());
            }
        }    
    }

    private void OnEnable()
    {

        MouseActivatedInPanel();
    }

    public void ExitPanel()
    {
        InventoryPannel.SetActive(false);
    }

    public void ShowNote() 
    {
        int numberOfNotes = inventoryManager.quantityOfNotes;
        for (int i = 0; i < numberOfNotes ; i++)
        {
            transform.GetChild(4).gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
        transform.GetChild(4).gameObject.SetActive(true);
    }

    IEnumerator DontShowNote() 
    {
        yield return new WaitForSeconds(3);
        transform.GetChild(4).gameObject.SetActive(false); 
    }




}
